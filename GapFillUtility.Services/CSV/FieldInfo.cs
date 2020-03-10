using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GapFillUtility.Services.CSV
{
    public class FieldInfo
    {
        [JsonConstructor]
        private FieldInfo()
        { }

        public FieldInfo(string name)
        : this(name, FieldType.String)
        {
        }

        public FieldInfo(string name, FieldType fieldType)
        {
            FieldName = name;
            FieldType = fieldType;
        }

        [JsonProperty("name")]
        public string FieldName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("type")]
        public FieldType FieldType { get; set; }
    }
}
