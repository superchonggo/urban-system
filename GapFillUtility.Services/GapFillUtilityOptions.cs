using CommandLine;
using GapFillUtility.Services.Interface;

namespace GapFillUtility.Services
{
    public class GapFillUtilityOptions : ILoaderOptions
    {
        [Option("Environment", Required = true,
    HelpText = "Target environment:\r\n   Development\r\n   Staging\r\n   Production")]
        public string Environment { get; set; }

        [Option("Version",
            Required = true, HelpText = "Product version, e.g. '04.03'")]
        public string ProductVersion { get; set; }

        [Option("Loaders", Required = true,
            HelpText = "At least one is required:\r\n   Asset\r\n   Crossref\r\n   " +
                       "Linkfarm\r\n   MappingSearch\r\n   " +
                       "Metadata\r\n   MultiFieldSearch\r\n   " +
                       "TypeaheadSearch\r\n\n   Multiple loaders can be joined with a comma:\r\n   e.g. AssetLoader,Metadata")]
        public string Loaders { get; set; }

        [Option("Products",
            Required = false, Default = "all", HelpText = "Comma separated values, e.g. jbjs, prcsgo")]
        public string Products { get; set; }
    }
}
