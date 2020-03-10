using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Serialization
{
    // Know how to serialize Vertices and Edges into CSV. Will use csvMapping to order properties and generate header files
    public class CsvSerializer
    {
        private readonly IContainerBuilder _containerBuilder;
        private readonly CSVMapping _csvMapping;

        public CsvSerializer(IContainerBuilder containerBuilder, CSVMapping csvMapping)
        {
            _containerBuilder = containerBuilder;
            _csvMapping = csvMapping;
        }

        public async Task Serialize(IEnumerable<Entity> entities)
        {
            // mark entities that already has headers
            var headersCreatedFor = new HashSet<string>();

            foreach (var entity in entities)
            {
                if (entity is Edge edge)
                {
                    await SerializeEdge(headersCreatedFor, edge);
                }
                else
                {
                    await SerializeVertex(headersCreatedFor, (Vertex)entity);
                }
            }
        }

        private async Task SerializeVertex(HashSet<string> headersCreatedFor, Vertex entity)
        {
            var mapping = _csvMapping.NodeMappings.FirstOrDefault(_ => _.Label == entity.Label);
            if (mapping == null) return;
            
            if (!headersCreatedFor.Contains(entity.Label))
            {
                await _containerBuilder.WriteHeader(entity.Label, sw => CsvSerializationExtensions.WriteNodeHeader(sw, mapping)).ConfigureAwait(false);
                headersCreatedFor.Add(entity.Label);
            }

            await _containerBuilder.WriteContent(entity.Label, sw => CsvSerializationExtensions.WriteNode(sw, mapping, entity)).ConfigureAwait(false);
        }

        private async Task SerializeEdge(HashSet<string> headersCreatedFor, Edge edge)
        {
            var mapping = _csvMapping.RelationMappings.FirstOrDefault(_ => _.Label == edge.Label);
            if (mapping == null) return;

            if (!headersCreatedFor.Contains(edge.Label))
            {
                await _containerBuilder.WriteHeader(edge.Label, sw => CsvSerializationExtensions.WriteRelationshipHeader(sw, mapping)).ConfigureAwait(false);
                headersCreatedFor.Add(edge.Label);
            }

            await _containerBuilder.WriteContent(edge.Label, sw => CsvSerializationExtensions.WriteRelationship(sw, mapping, edge)).ConfigureAwait(false);
        }
    }
}