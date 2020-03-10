using GapFillUtility.Services.Interface;
using NLog;
using Saxon.Api;
using System;
using System.IO;

namespace GapFillUtility.Services.Saxon
{
    public class SaxonTransformer : ITransformer
    {
        private readonly XsltTransformer _transformer;
        private readonly ILogger _logger;

        public SaxonTransformer(string strTransformerFilePath, ILogger logger)
        {
            _logger = logger;
            var processor = new Processor();
            using (var stream = File.OpenRead(strTransformerFilePath))
            {
                _transformer = processor.NewXsltCompiler().Compile(stream).Load();
            }
        }

        public void Transform(Stream inStream, Stream outStream)
        {
            // todo: fix the empty constructor for serializer            

            //_logger.LogTrace($"Running transformation");
            var dest = new Serializer();
            dest.SetOutputStream(outStream);
            _transformer.SetInputStream(inStream, new Uri("http://no.where.to.go/"));
            _transformer.Run(dest);
        }
    }
}
