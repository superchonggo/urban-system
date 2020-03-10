using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Parsers;

namespace GapFillUtility.Services.Interface
{
    public interface IMappingProvider
    {
        CSVMapping GetMappingByData(ParsedData data);
    }
}
