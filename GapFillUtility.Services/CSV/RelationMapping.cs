using Newtonsoft.Json;
using System.Collections.Generic;

namespace GapFillUtility.Services.CSV
{
    public class RelationMapping
    {
        [JsonConstructor]
        private RelationMapping()
        {
            FieldInfos = new List<FieldInfo>();
        }

        public RelationMapping(FieldInfo start, FieldInfo end, string label, params FieldInfo[] fieldInfos)
        : this()
        {
            Label = label ?? throw new System.ArgumentException("message", nameof(label));
            StartIdField = start ?? throw new System.ArgumentNullException(nameof(start));
            EndIdField = end ?? throw new System.ArgumentNullException(nameof(end));

            foreach (var fieldInfo in fieldInfos)
            {
                FieldInfos.Add(fieldInfo);
            }
        }

        [JsonProperty("type")]
        public string Label { get; set; }
        [JsonProperty("start")]
        public FieldInfo StartIdField { get; set; }
        [JsonProperty("end")]
        public FieldInfo EndIdField { get; set; }
        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public IList<FieldInfo> FieldInfos { get; set; }
    }
}
