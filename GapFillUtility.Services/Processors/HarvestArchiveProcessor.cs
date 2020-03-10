using GapFillUtility.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Processors
{
    public class HarvestArchiveProcessor : IArchiveProcessor
    {
        private readonly ITransformer _transformer;

        public HarvestArchiveProcessor(ITransformer transformer)
        {
            _transformer = transformer;
        }

        public async Task<IEnumerable<Tuple<Stream, string>>> ProcessArchive(ZipArchive archive)
        {
            var result = new List<Tuple<Stream, string>>();

            foreach (var entry in archive.Entries)
            {
                if (!entry.FullName.EndsWith(".xml")) continue;

                var memoryStream = new MemoryStream();
                using (var assetData = entry.Open())
                {
                    _transformer.Transform(assetData, memoryStream);
                }

                result.Add(new Tuple<Stream, string>(memoryStream, entry.FullName));
            }

            return result;
        }
    }
}
