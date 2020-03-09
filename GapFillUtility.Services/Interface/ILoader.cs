using GapFillUtility.Services.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Interface
{
    public interface ILoader
    {
        string BuildHarvestingServiceUrl(string productId, string productVersion, string fromDate);
        void SendMessages(IEnumerable<Uri> assetUrls, string productId, string productVersion);
        Task DownloadFile(AssetModel item);
        // string ExtractContentSetId(string strUri);
        void ExtractHarvestFiles();
        void Initialize(ILoaderOptions options);

        IEnumerable<string> GetFGRContent(ConfigurationSettings configSettings, string contentType, string productVersion);

        Task GenerateCSV();
        Task GenerateMessage();

        Task<HarvestList> GetHarvestList(string uri);

        // Task<IEnumerable<Uri>> HarvestList(string strServiceUri);       
        Task SendMessage(LoadRequest loadRequest, string loaderName, bool isSuccess);
    }
}
