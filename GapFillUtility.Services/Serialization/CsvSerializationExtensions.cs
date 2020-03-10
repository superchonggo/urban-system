using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Serialization
{
    public static class CsvSerializationExtensions
    {
        internal const char DELIMITER = '|';

        public static async Task WriteNodeHeader(TextWriter writer, NodeMapping nodeMapping)
        {
            var isFirst = true;
            foreach (var mapping in nodeMapping.FieldInfos)
            {
                if (!isFirst)
                    await writer.WriteAsync(DELIMITER).ConfigureAwait(false);
                isFirst = false;

                if (mapping == nodeMapping.IdField)
                    await writer.WriteAsync(mapping.FieldName).ConfigureAwait(false);
                else
                {
                    await WriteFieldInfo(writer, mapping).ConfigureAwait(false);
                }
            }

            await writer.WriteLineAsync().ConfigureAwait(false);
        }

        public static async Task WriteNode(TextWriter writer, NodeMapping nodeMapping, Entity node)
        {
            var isFirst = true;
            foreach (var fieldInfo in nodeMapping.FieldInfos)
            {
                if (!isFirst)
                    await writer.WriteAsync(DELIMITER).ConfigureAwait(false);
                isFirst = false;

                if (fieldInfo == nodeMapping.IdField)
                    await WriteField(writer, fieldInfo, node.Key).ConfigureAwait(false);
                else
                {
                    if (!node.Properties.ContainsKey(fieldInfo.FieldName))
                        await WriteField(writer, fieldInfo, null).ConfigureAwait(false);
                    else
                        await WriteField(writer, fieldInfo, node.Properties[fieldInfo.FieldName]).ConfigureAwait(false);
                }
            }
            await writer.WriteLineAsync().ConfigureAwait(false);
        }

        public static async Task WriteRelationshipHeader(TextWriter writer, RelationMapping relationMapping)
        {
            await writer.WriteAsync($"{relationMapping.StartIdField.FieldName}{DELIMITER}").ConfigureAwait(false);
            await writer.WriteAsync($"{relationMapping.EndIdField.FieldName}").ConfigureAwait(false);

            foreach (var fieldInfo in relationMapping.FieldInfos)
            {
                await writer.WriteAsync(DELIMITER).ConfigureAwait(false);
                await WriteFieldInfo(writer, fieldInfo).ConfigureAwait(false);
            }

            await writer.WriteLineAsync().ConfigureAwait(false);
        }

        public static async Task WriteRelationship(TextWriter writer, RelationMapping relationMapping, Edge edge)
        {
            await WriteField(writer, relationMapping.StartIdField, edge.OutKey).ConfigureAwait(false);
            await writer.WriteAsync(DELIMITER).ConfigureAwait(false);
            await WriteField(writer, relationMapping.EndIdField, edge.InKey).ConfigureAwait(false);

            foreach (var fieldInfo in relationMapping.FieldInfos)
            {
                await writer.WriteAsync(DELIMITER).ConfigureAwait(false);
                await WriteField(writer, fieldInfo, edge.Properties[fieldInfo.FieldName]).ConfigureAwait(false);
                
            }

            await writer.WriteLineAsync().ConfigureAwait(false);
        }

        private static async Task WriteFieldInfo(TextWriter writer, FieldInfo fieldInfo)
        {
            await writer.WriteAsync(fieldInfo.FieldName).ConfigureAwait(false);
        }

        private static async Task WriteField(TextWriter writer, FieldInfo fieldInfo, string value)
        {
            if (value == null) return;

            switch (fieldInfo.FieldType)
            {
                case FieldType.Long:
                    await writer.WriteAsync(value).ConfigureAwait(false);
                    break;
                case FieldType.Boolean:
                    await writer.WriteAsync(value).ConfigureAwait(false);
                    break;
                case FieldType.String:
                    await writer.WriteAsync(value).ConfigureAwait(false);
                    break;
                default:
                    throw new NotImplementedException($"Type {fieldInfo.FieldType} is not supported");
            }
        }
    }
}