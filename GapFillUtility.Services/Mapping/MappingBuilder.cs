using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Transformation;

namespace GapFillUtility.Services.Mapping
{
    public class MappingBuilder
    {
        private readonly AssetType _assetType;

        public MappingBuilder(AssetType assetType)
        {
            _assetType = assetType;
        }

        public CSVMapping Build()
        {
            var mappingProvider = GetMappingProvider();

            return new CSVMapping(
                mappingProvider.GetCsvRelationMapping(),
                mappingProvider.GetCsvNodeMapping(),
                mappingProvider.GetPostProcessesAction());
        }

        private ICsvMappingConfiguration GetMappingProvider()
        {
            switch (_assetType)
            {
                case AssetType.Issue:
                    return new CsvIssueMappingConfiguration();
                case AssetType.Article:
                    return new CsvArticleMappingConfiguration();
                case AssetType.Product:
                    return new CsvProductMappingConfiguration();
                case AssetType.Topic:
                    return new CsvTopicMappingConfiguration();
                default:
                    // Like cover and other artifacts
                    return new CsvArticleMappingConfiguration();
            }
        }
    }
}
