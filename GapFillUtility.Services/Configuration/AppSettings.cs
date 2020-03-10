namespace GapFillUtility.Services.Configuration
{
    public class AppSettings
    {
        public AppSettings(
            ServiceBusSettings serviceBusSettings,
            ConfigurationSettings configurationSettings)
        {
            ServiceBusSettings = serviceBusSettings;
            ConfigurationSettings = configurationSettings;
        }


        public ServiceBusSettings ServiceBusSettings { get; }
        public ConfigurationSettings ConfigurationSettings { get; }
    }

    public enum AssetType
    {
        Default,
        Issue,
        Article,
        Topic,
        Product
    }
}
