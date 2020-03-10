using GapFillUtility.Services.CSV;

namespace GapFillUtility.Services.Mapping
{
    public interface ICsvMappingConfiguration
    {
        NodeMapping[] GetCsvNodeMapping();
        RelationMapping[] GetCsvRelationMapping();
        string[] GetPostProcessesAction();
    }
}
