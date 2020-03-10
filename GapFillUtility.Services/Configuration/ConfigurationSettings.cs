namespace GapFillUtility.Services.Configuration
{
    public static class ConfigurationConstants
    {
        public const string APP_SETTINGS_JSON = "appsettings.json";
        public const string SERVICE_BUS_SECTION = "ServiceBus";
        public const string CONFIGURATION_SECTION = "ConfigurationSettings";
    }

    public class ConfigurationSettings
    {
        public string UrlTemplateMissing { get; set; }
        public string UrlTemplateGapFill { get; set; }
        public string UrlTemplateGapFillTemplate { get; set; }
        public string Cookie { get; set; }
        public string UrlTemplate { get; set; }
        public string UrlTemplateBase { get; set; }
        public string AssetOfType { get; set; }
        public string ProductVersion { get; set; }
        public string PathWKMRIDS { get; set; }
        public string PathURLS { get; set; }
        public string JournalListPath { get; set; }
        public string MissingListPath { get; set; }
        public string SubscriptionKey { get; set; }
        public string BlobConnectionString { get; set; }
        public string ReprocessProductUpdatesUrl { get; set; }
        public string HarvestServiceSecretToByPassProductStatusCheck { get; set; }
        public string SaxonTransformationFile { get; set; }
        public string UrlTemplateVersion
        {
            get
            {
                return $"{UrlTemplateBase}{ProductVersion}";
            }
        }
    }

    public class ServiceBusSettings
    {
        public string ConnectionString { get; set; }
    }
}
