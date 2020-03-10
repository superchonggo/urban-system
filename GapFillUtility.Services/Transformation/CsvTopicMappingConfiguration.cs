using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Transformation
{
    public class CsvTopicMappingConfiguration : ICsvMappingConfiguration
    {
        public NodeMapping[] GetCsvNodeMapping()
        {
            return new[]
            {
                new NodeMapping(
                    new FieldInfo("id", FieldType.Long),
                    "Category",
                    new FieldInfo("keyValue"),
                    new FieldInfo("value"),
                    new FieldInfo("firstLetter")),
                new NodeMapping(
                    new FieldInfo("id", FieldType.Long),
                    "Author",
                    new FieldInfo("keyValue"),
                    new FieldInfo("value"),
                    new FieldInfo("lastName"),
                    new FieldInfo("firstName"),
                    new FieldInfo("degree")),
                new NodeMapping(
                    new FieldInfo("id", FieldType.Long),
                    "Topic",
                    new FieldInfo("keyValue"),
                    new FieldInfo("value"),
                    new FieldInfo("apolloId"),
                    new FieldInfo("title"),
                    new FieldInfo("firstLetter")),
                new NodeMapping(
                    new FieldInfo("id", FieldType.Long),
                    "Specialty",
                    new FieldInfo("keyValue"),
                    new FieldInfo("value"),
                    new FieldInfo("firstLetter"))
            };
        }

        public RelationMapping[] GetCsvRelationMapping()
        {
            return new[]
            {
                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "BY_AUTHOR"),
                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_TOPIC"),
                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_SPECIALTY")
            };
        }

        public string[] GetPostProcessesAction()
        {
            return null;
        }
    }
}
