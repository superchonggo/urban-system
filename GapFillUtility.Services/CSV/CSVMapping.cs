using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GapFillUtility.Services.CSV
{
    public class CSVMapping
    {
        [JsonConstructor]
        public CSVMapping()
        { }

        public CSVMapping(IEnumerable<RelationMapping> relationMappings, IEnumerable<NodeMapping> nodeMappings, IEnumerable<string> postActions = null)
        {
            RelationMappings = relationMappings ?? throw new System.ArgumentNullException(nameof(relationMappings));
            NodeMappings = nodeMappings ?? throw new System.ArgumentNullException(nameof(nodeMappings));
            PostActions = postActions ?? new string[0];
        }

        [JsonProperty("node")]
        public IEnumerable<NodeMapping> NodeMappings { get; private set; }

        [JsonProperty("edges")]
        public IEnumerable<RelationMapping> RelationMappings { get; private set; }

        [JsonProperty("postActions")]
        public IEnumerable<string> PostActions { get; private set; }
    }
}
