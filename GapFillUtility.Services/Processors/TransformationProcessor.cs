using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Interface;
using GapFillUtility.Services.Parsers;
using GapFillUtility.Services.Serialization;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Processors
{
    public class TransformationProcessor : ITransformationProcessor
    {
        private readonly ITransformer _transformer;
        private readonly IDownloader _downloader;
        private readonly IZipProcessor _zipProcessor;
        private readonly IMappingProvider _mappingProvider;

        public TransformationProcessor(ITransformer transformer
            , IDownloader downloader
            , IZipProcessor zipProcessor
            , IMappingProvider mappingProvider)
        {
            _transformer = transformer;
            _downloader = downloader;
            _zipProcessor = zipProcessor;
            _mappingProvider = mappingProvider;
        }

        public async Task ProcessAsync(Uri uri, Stream outputStream)
        {
            using (var outputZip = new ZipArchive(outputStream, ZipArchiveMode.Create, leaveOpen: true))
            using (var containerBuilder = new ContainerBuilder(outputZip))
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                CSVMapping csvMapping = null;
                var streamParser = new StreamParser();
                var file = _downloader.GetFile(uri);
                var zip = _zipProcessor.UnZip(file);
                foreach (var zipEntry in zip.Entries)
                {
                    if (!zipEntry.FullName.ToLower().EndsWith(".xml")) continue;
                    using (var inputStream = zipEntry.Open())
                    using (var stream = new MemoryStream())
                    {
                        _transformer.Transform(inputStream, stream);
                        if (stream.Length == 0) continue;
                        stream.Position = 0;
                        var data = await streamParser.Parse(stream);

                        csvMapping = _mappingProvider.GetMappingByData(data);
                        var serializer = new CsvSerializer(containerBuilder, csvMapping);

                        await serializer.Serialize(data.ParsedEntities);
                    }
                }


                // Add metadata to each archive
                if (csvMapping != null)
                {
                    CSVMappingHelper.AddMetadata(outputZip, csvMapping);
                }

                stopWatch.Stop();

                var ts = stopWatch.Elapsed;
                var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
        }
    }
}
