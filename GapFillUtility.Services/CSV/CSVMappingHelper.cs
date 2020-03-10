using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GapFillUtility.Services.CSV
{
    public static class CSVMappingHelper
    {
        private const string MetadataJson = "metadata.json";

        public static CSVMapping Load(string input)
        {
            return JsonConvert.DeserializeObject<CSVMapping>(input);
        }

        public static string Save(CSVMapping mapping)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };
            return JsonConvert.SerializeObject(mapping, Formatting.None, settings);
        }

        [Obsolete("Please use configuration based mapping. This method would be removed in next iterrations")]
        public static CSVMapping GetDummyData()
        {
            return new CSVMapping(
                new RelationMapping[]
                {
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_ARTICLE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "PUB_ON_DATE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_DATE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IN_MONTH"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_MONTH"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IN_YEAR"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "WROTE_ARTICLE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "BY_AUTHOR"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "OF_PRODUCT"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_PRODVERSION"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "PROD_OF_JOURNAL"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IS_PRODTYPE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_PRODUCT"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_REP"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "OF_ARTICLE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IN_PRODVERSION"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_PISSN"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_ISSN"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_ISSUE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IN_ISSUE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "PUB_IN_YEAR"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "OF_ISSUE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "OF_JOURNAL"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "OF_VOLUME"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_PAGE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_JOURNAL"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IN_LANGUAGE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "ON_PAGE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IN_VOLUME"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "IN_JOURNAL"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_VOLUME"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "COVER_OF_ISSUE"),
                    new RelationMapping(new FieldInfo("out", FieldType.Long), new FieldInfo("in", FieldType.Long), "HAS_COVER_DATE")
                },
               new NodeMapping[]
                {
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Product", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "ProductType", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Representation", new FieldInfo("keyValue"), new FieldInfo("value"), new FieldInfo("qualifier"),new FieldInfo("fileSize"), new FieldInfo("status")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "ProductVersion", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Article", new FieldInfo("keyValue"), new FieldInfo("value"), new FieldInfo("accessionNumber"), new FieldInfo("doi"), new FieldInfo("subTitle"), new FieldInfo("firstPage"),new FieldInfo("lastPage"),new FieldInfo("pageRange"),new FieldInfo("publicationDate"),new FieldInfo("openAccess")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Page", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Date", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Month", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Year", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Author",new FieldInfo("keyValue"), new FieldInfo("value"), new FieldInfo("lastName"), new FieldInfo("firstName"), new FieldInfo("degree")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Issue", new FieldInfo("keyValue"), new FieldInfo("value"), new FieldInfo("issueId"), new FieldInfo("accessionNumber"), new FieldInfo("supplementNumber"),new FieldInfo("shortCode"),new FieldInfo("calFieldsUpdated", FieldType.Boolean)),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Volume", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Journal",new FieldInfo("keyValue"), new FieldInfo("value"),new FieldInfo("normalizedTitle"), new FieldInfo("firstLetter"),  new FieldInfo("hwp"), new FieldInfo("nlmTa"), new FieldInfo("publisherId"),new FieldInfo("accessionNumber"),new FieldInfo("shortCode")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Issn", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Language", new FieldInfo("keyValue"), new FieldInfo("value")),
                    new NodeMapping(new FieldInfo("id", FieldType.Long), "Multimedia", new FieldInfo("keyValue"), new FieldInfo("value"))
                },
                new[]
                {
                    "AcademicContentGraph.UpdateCalculatedFields"
                });
        }

        public static void AddMetadata(this ZipArchive archive, CSVMapping mapping)
        {
            var metadataEntry = archive.CreateEntry(MetadataJson);
            using (var stream = metadataEntry.Open())
            using (StreamWriter writer = new StreamWriter(stream))
            {
                var serializedMapping = CSVMappingHelper.Save(mapping);
                writer.Write(serializedMapping);
            }
        }

        public static async Task<CSVMapping> RestoreMetadata(this ZipArchive archive)
        {
            var metadataEntry = archive.GetEntry(MetadataJson);
            using (var stream = metadataEntry.Open())
            using (var reader = new StreamReader(stream))
            {
                var serializedMapping = await reader.ReadToEndAsync();
                return CSVMappingHelper.Load(serializedMapping);
            }
        }
    }
}
