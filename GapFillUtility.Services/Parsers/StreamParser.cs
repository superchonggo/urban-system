using GapFillUtility.Services.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Parsers
{
    public interface IStreamParser
    {
        Task<ParsedData> Parse(Stream stream);
    }

    public class StreamParser : IStreamParser
    {
        private const string PartitionKeyValueName = "partitionKeyValue";
        private const string HeaderPrefix = "#!";
        private static readonly int HeaderPrefixSize = HeaderPrefix.Length;
        private string PRODUCT_NAME_VARIABLE = "ProductName";
        private string PRODUCT_TYPE_VARIABLE = "ProductType";
        private string PRODUCT_VERSION_VARIABLE = "ProductVersion";
        private string SOURCE_ASSET_TYPE_VARIABLE = "SourceAssetType";

        public async Task<ParsedData> Parse(Stream stream)
        {
            if (stream.Length == 0)
                return null;
            using (var reader = new StreamReader(stream))
            {
                var header = await ReadHeader(reader);
                var context = new ParsingContext()
                {
                    ProductName = header[PRODUCT_NAME_VARIABLE],
                    ProductType = header.ContainsKey(PRODUCT_TYPE_VARIABLE) ? header[PRODUCT_TYPE_VARIABLE] : "",
                    ProductVersion = header.ContainsKey(PRODUCT_VERSION_VARIABLE) ? header[PRODUCT_VERSION_VARIABLE] : "",
                    SourceAssetType = header[SOURCE_ASSET_TYPE_VARIABLE],
                };
                return new ParsedData()
                {
                    ProductName = context.ProductName,
                    ProductType = context.ProductType,
                    ProductVersion = context.ProductVersion,
                    SourceAssetType = context.SourceAssetType,
                    ParsedEntities = await ReadEntities(reader, context)
                };
            }
        }

        private async Task<IEnumerable<Entity>> ReadEntities(StreamReader reader, ParsingContext context)
        {
            var result = new List<Entity>();
            do
            {
                var line = await reader.ReadLineAsync();
                var entity = ParseEntity(line, context);
                if (entity != null)
                    result.Add(entity);
            } while (!reader.EndOfStream);

            return result;
        }

        private Entity ParseEntity(string line, ParsingContext context)
        {
            if (line.StartsWith("E ", StringComparison.InvariantCultureIgnoreCase))
                return ParseEdge(line, context);
            else if (line.StartsWith("V ", StringComparison.InvariantCultureIgnoreCase))
                return ParseVertex(line, context);
            else
            {
                // skip, not supported. Help deal with noise from Extraction step
                return null;
            }
        }

        private Entity ParseVertex(string line, ParsingContext context)
        {
            var vertex = new Vertex()
            {
                Key = EntityParser.ReadKey(line),
                Label = EntityParser.ReadLabel(line),
                Properties = EntityParser.ReadProperties(line)
            };

            return vertex;
        }

        private Entity ParseEdge(string line, ParsingContext context)
        {
            var edge = new Edge()
            {
                Key = EntityParser.ReadKey(line),
                Label = EntityParser.ReadLabel(line),
                InKey = EntityParser.ReadInKey(line),
                OutKey = EntityParser.ReadOutKey(line),
                Properties = EntityParser.ReadProperties(line)
            };

            return edge;
        }

        private async Task<Dictionary<string, string>> ReadHeader(StreamReader reader)
        {
            var result = new Dictionary<string, string>();
            while ((char)reader.Peek() == HeaderPrefix[0])
            {
                // Expected format is #!key=value
                var line = await reader.ReadLineAsync();
                var delimiterIndex = line.IndexOf('=');
                var key = line.Substring(HeaderPrefixSize, delimiterIndex - HeaderPrefixSize);
                var value = line.Substring(delimiterIndex + 1, line.Length - (delimiterIndex + 1));

                result.Add(key, value);
            }

            return result;
        }

        private class ParsingContext
        {
            public ParsingContext()
            {
                KeyToPartitionKeyMapper = new Dictionary<string, string>();
            }

            public string ProductName { get; set; }
            public string ProductVersion { get; set; }
            public string ProductType { get; set; }
            public string SourceAssetType { get; set; }

            public Dictionary<string, string> KeyToPartitionKeyMapper { get; private set; }
        }
    }
}
