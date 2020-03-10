using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GapFillUtility.Services.CSV
{
    public class NodeMapping
    {
        [JsonConstructor]
        private NodeMapping()
        {
            FieldInfos = new List<FieldInfo>();
        }

        public NodeMapping(FieldInfo idField, string label, params FieldInfo[] fieldInfos)
        : this()
        {
            if (idField == null)
                throw new ArgumentNullException(nameof(idField));
            if (string.IsNullOrEmpty(label))
                throw new ArgumentNullException(nameof(label));

            Label = label;
            IdField = idField;
            FieldInfos.Add(IdField);
            foreach (var fieldInfo in fieldInfos)
            {
                FieldInfos.Add(fieldInfo);
            }
        }

        [JsonProperty("label", Required = Required.Always)]
        public string Label { get; set; }
        [JsonProperty("id", Required = Required.Always)]
        public FieldInfo IdField { get; set; }
        [JsonProperty("fields", NullValueHandling = NullValueHandling.Ignore)]
        public IList<FieldInfo> FieldInfos { get; private set; }
    }
}
