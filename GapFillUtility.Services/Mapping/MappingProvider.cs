using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Interface;
using GapFillUtility.Services.Parsers;
using System;

namespace GapFillUtility.Services.Mapping
{
    public class MappingProvider : IMappingProvider
    {
        public CSVMapping GetMappingByData(ParsedData data)
        {
            Enum.TryParse(data.SourceAssetType, true, out AssetType assetType);
            var builder = new MappingBuilder(assetType);
            return builder.Build();
        }
    }
}
