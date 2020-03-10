using GapFillUtility.Services.CSV;

namespace GapFillUtility.Services.Transformation
{
    public sealed class CsvIssueMappingConfiguration : CsvArticleMappingConfiguration
    {

        protected override NodeMapping GetIssueNodeMappings()
        {
            var baseNodeMappingIssue = base.GetIssueNodeMappings();
            baseNodeMappingIssue.FieldInfos.Add(new FieldInfo("wkmrid"));
            return baseNodeMappingIssue;
        }
    }
}
