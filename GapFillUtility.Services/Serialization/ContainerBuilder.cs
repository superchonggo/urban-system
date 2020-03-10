using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Serialization
{

    // Responsible for creating Container with correct hierarchy
    public class ContainerBuilder : IContainerBuilder
    {
        private const string DefaultExtension = ".csv";
        private readonly Dictionary<string, EntryData> _contentStreams = new Dictionary<string, EntryData>();
        private readonly ZipArchive _container;

        public ContainerBuilder(ZipArchive container)
        {
            _container = container;
        }

        public async Task WriteHeader(string entityId, Func<TextWriter, Task> headersWriter)
        {
            StreamWriter writer = null;
            if (!_contentStreams.ContainsKey(entityId))
            {
                _contentStreams.Add(entityId, new EntryData($"{entityId}{DefaultExtension}"));
            }

            // Write headers only once
            var entry = _contentStreams[entityId];
            if (entry.HeaderUpdated) return;
            entry.HeaderUpdated = true;

            writer = entry.HeaderWriter;
            await headersWriter(writer).ConfigureAwait(false);
        }

        public async Task WriteContent(string entityId, Func<TextWriter, Task> contentWriter)
        {
            StreamWriter writer = null;
            if (!_contentStreams.ContainsKey(entityId))
            {
                _contentStreams.Add(entityId, new EntryData($"{entityId}{DefaultExtension}"));
            }
            
            writer = _contentStreams[entityId].ContentWriter;
            await contentWriter(writer).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Flush();
            _container?.Dispose();
        }

        private void Flush()
        {
            // Dispose streams and then dispose container
            foreach (var kv in _contentStreams)
            {
                using (var outputStream = _container.CreateEntry(kv.Value.EntryName).Open())
                {
                    kv.Value.Flush(outputStream);
                }

                kv.Value.Dispose();
            }
            _contentStreams.Clear();
        }

        private class EntryData: IDisposable
        {
            public EntryData(string entryName)
            {
                EntryName = entryName;
                ConentStream = new MemoryStream();
                ContentWriter = new StreamWriter(ConentStream);

                HeaderStream = new MemoryStream();
                HeaderWriter = new StreamWriter(HeaderStream);

                HeaderUpdated = false;
            }

            public string EntryName { get; private set; }

            public bool HeaderUpdated { get; set; }

            public StreamWriter HeaderWriter { get; private set; }
            public StreamWriter ContentWriter { get; private set; }

            private Stream HeaderStream { get; set; }
            private Stream ConentStream { get; set; }

            public void Flush(Stream outputStream)
            {
                HeaderWriter.Flush();
                HeaderStream.Position = 0;
                HeaderStream.CopyTo(outputStream);

                ContentWriter.Flush();
                ConentStream.Position = 0;
                ConentStream.CopyTo(outputStream);
            }

            public void Dispose()
            {
                HeaderWriter.Dispose();
                HeaderStream.Dispose();
                ContentWriter.Dispose();
                ConentStream.Dispose();
            }
        }
    }
}