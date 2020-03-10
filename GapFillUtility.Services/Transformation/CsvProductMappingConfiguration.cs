using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Transformation
{
    public class CsvProductMappingConfiguration : ICsvMappingConfiguration
    {
        public NodeMapping[] GetCsvNodeMapping()
        {
            return new[]
                {
                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "ProductVersion",
                        new FieldInfo("keyValue"),
                        new FieldInfo("frequency"),
                        new FieldInfo("value"),
                        new FieldInfo("status")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Category",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Subject",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Publisher",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Journal",
                        new FieldInfo("keyValue"),
                        new FieldInfo("shortCode"),
                        new FieldInfo("openAccessDescriptor"),
                        new FieldInfo("normalizedTitle"),
                        new FieldInfo("firstLetter"),
                        new FieldInfo("value"),
                        new FieldInfo("openAccessStartCoverageDate"),
                        new FieldInfo("openAccessEndCoverageDate"),
                        new FieldInfo("openAccessMode")),

                        new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Year",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Date",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Month",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Product",
                        new FieldInfo("keyValue"),
                        new FieldInfo("description"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "ProductType",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),

                    new NodeMapping(
                        new FieldInfo("id", FieldType.Long),
                        "Issn",
                        new FieldInfo("keyValue"),
                        new FieldInfo("value")),
                };
        }

        public RelationMapping[] GetCsvRelationMapping()
        {
            return new[]
            {
                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_PRODUCT"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "IS_PRODTYPE"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_PRODVERSION"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "OF_PRODUCT"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_PUBLISHER"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_SUBJECT"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_CATEGORY"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_START_COVERAGE_DATE"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "IN_MONTH"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_DATE"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "IN_YEAR"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_MONTH"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_END_COVERAGE_DATE"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "PRECEDED_BY"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_PISSN"),

                new RelationMapping(
                    new FieldInfo("out", FieldType.Long),
                    new FieldInfo("in", FieldType.Long),
                    "HAS_EISSN")
            };
        }

        public string[] GetPostProcessesAction()
        {
            return null;
        }
    }
}
