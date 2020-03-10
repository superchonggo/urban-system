using CommandLine;
using GapFillUtility.Services;
using GapFillUtility.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceBus;
using NLog;
using System;
using System.Linq;

namespace GapFillUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var parser = new Parser(settings =>
                {
                    settings.CaseSensitive = false;
                    settings.HelpWriter = null;
                });

                var parserResults = parser.ParseArguments<GapFillUtilityOptions>(args);
                parserResults
                    .WithParsed(Run)
                    .WithNotParsed(errors =>
                    {
                        Helpers.HighlightText($"The parser failed to interpret the command. Number error/s found: {errors.Count()}", false);

                        Helpers.DisplayHelp(parserResults, errors);
                    });
            }
            catch (Exception e)
            {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ForegroundColor = defaultColor;
            }
        }

        private static void Run(GapFillUtilityOptions options)
        {
            // https://confluence.wolterskluwer.io/display/HLRPPlatform/Loader+Infrastructure
            // Mapping Loader
            // Input: unzipped CSV files.These files were transformed from AssetXml to CSV by the SSR Mapping loader, using an XSL file that was provided by the PS team.
            // Output: Mapping - content loaded into Neo4j for the mapping feature.

            Console.Title = "GapFillContent";

            var configuration = InitializeConfiguration();
            var appSettings = CreateAppSettings(configuration);
            var services = BootstrapIoC(appSettings);

            var builder = new ServiceBusConnectionStringBuilder(appSettings.ServiceBusSettings.ConnectionString);
            var logger = services.GetRequiredService<ILogger>();

            new GapFillUtilityExecutor(appSettings, logger).Initialize(options);

            Console.ReadLine();
        }

        #region "Startup"
        private static IConfiguration InitializeConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(
                    ConfigurationConstants.APP_SETTINGS_JSON,
                    optional: false,
                    reloadOnChange: true);

            return configurationBuilder.Build();
        }

        private static AppSettings CreateAppSettings(IConfiguration configuration)
        {
            var serviceBusSettings = new ServiceBusSettings();
            configuration.GetSection(ConfigurationConstants.SERVICE_BUS_SECTION)
                .Bind(serviceBusSettings);

            var configurationSettings = new ConfigurationSettings();
            configuration.GetSection(ConfigurationConstants.CONFIGURATION_SECTION)
                .Bind(configurationSettings);

            return new AppSettings(
                serviceBusSettings,
                configurationSettings);
        }

        public static IServiceProvider BootstrapIoC(AppSettings appSettings)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ILogger>
                (f => LogManager.GetCurrentClassLogger());

            return serviceCollection.BuildServiceProvider();
        }
        #endregion               
    }
}
