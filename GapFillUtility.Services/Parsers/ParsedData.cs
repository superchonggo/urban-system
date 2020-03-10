using GapFillUtility.Services.Model;
using System.Collections.Generic;

namespace GapFillUtility.Services.Parsers
{
    public class ParsedData
    {
        public ParsedData()
        {
            ProductName = "";
            ProductVersion = "";
            ProductType = "";
            SourceAssetType = "";
        }

        public string ProductName { get; set; }
        public string ProductVersion { get; set; }
        public string ProductType { get; set; }
        public string SourceAssetType { get; set; }

        public IEnumerable<Entity> ParsedEntities { get; set; }

        public override bool Equals(object obj)
        {
            var data = obj as ParsedData;
            return data != null &&
                   ProductName == data.ProductName &&
                   ProductVersion == data.ProductVersion &&
                   ProductType == data.ProductType &&
                   SourceAssetType == data.SourceAssetType &&
                   EqualityComparer<IEnumerable<Entity>>.Default.Equals(ParsedEntities, data.ParsedEntities);
        }

        public override int GetHashCode()
        {
            var hashCode = 276563190;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProductName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProductVersion);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ProductType);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SourceAssetType);
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Entity>>.Default.GetHashCode(ParsedEntities);
            return hashCode;
        }
    }
}
