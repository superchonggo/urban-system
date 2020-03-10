using GapFillUtility.Services.Configuration;
using GapFillUtility.Services.CSV;
using GapFillUtility.Services.Interface;
using GapFillUtility.Services.Model;
using GapFillUtility.Services.Parsers;
using GapFillUtility.Services.Saxon;
using GapFillUtility.Services.Serialization;
using Newtonsoft.Json;
using NLog;
using Saxon.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GapFillUtility.Services
{
    public class GapFillUtilityExecutor : ILoader
    {
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly string _storagePath;
        private readonly ITransformer _transformer;
        // private readonly IZipProcessor _zipProcessor;
        private readonly IMappingProvider _mappingProvider;


        public GapFillUtilityExecutor(AppSettings appSettings, ILogger logger)
        {
            _appSettings = appSettings;
            _logger = logger;
            _httpClient = new HttpClient();
            _storagePath = Directory.CreateDirectory($@"C:\Projects\assetfiles\{Helpers.GetUniqueDateStampAudit()}\").FullName;           
            _transformer = new SaxonTransformer("./Asset_To_CSV.xsl", _logger);

            //// todo: make this dynamic
            //using (var stream = File.OpenRead("./Asset_To_CSV.xsl"))
            //{
            //    _transformer = processor.NewXsltCompiler().Compile(stream).Load();
            //}
        }

        public string BuildHarvestingServiceUrl(string productId, string productVersion, string fromDate)
        {
            var url = string.Format(_appSettings.ConfigurationSettings.UrlTemplateGapFill, productId, productVersion);

            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                url += $"&from={fromDate}";
            }

            return url;
        }

        public async Task DownloadFile(AssetModel item)
        {
            try
            {
                var fileName = item.ContentUri.GetAssetFileName();

                using (var fs = new FileStream($@"{_storagePath}{fileName}", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var response = await _httpClient.GetAsync(item.ContentUri);
                    await response.Content.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ExtractHarvestFiles()
        {
            throw new NotImplementedException();
        }

        private async Task FillGapsAsync(IEnumerable<string> lstProducts, string productVersion)
        {
            // Begin timing.
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var lstErrors = new List<string>();
            var lstProcessed = new List<string>();

            Helpers.DisplayHeader("GapFill Journals");

            var totalGapFill = 0;

            foreach (var product in lstProducts)
            {
                // todo: validate null input for date
                var requestUri = BuildHarvestingServiceUrl(product, _appSettings.ConfigurationSettings.ProductVersion, null);

                try
                {
                    // todo: figure out why this isn't working without the .Result
                    var reprocessMessageList = await HarvestList(requestUri);

                    Helpers.DisplayHeader($"{reprocessMessageList.Count} files harvested");

                    foreach (var item in reprocessMessageList)
                    {
                        //DownloadFile(item).Wait();
                        await DownloadFile(item);
                    }

                    Helpers.DisplayHeader($"Completed downloading files");

                    await GenerateCSV();

                    //var dirInfo = new DirectoryInfo(_storagePath);

                    //if (reprocessMessageList.Count == dirInfo.GetFiles().Length)
                    //{
                    //    Console.WriteLine("same number of files");
                    //}


                    //foreach (var item in reprocessMessage)
                    //{
                    //    if (i == 5)
                    //        break;

                    //    ++i;

                    //    // todo: rewrite this in a way it will consume assetmodel
                    //    DownloadFile(item).Wait();
                    //}

                    // Helpers.SetConsoleHeader($"Processing {reprocessMessage.ContentSetIds.Count()} messages from {product}");

                    //    var results = await PostMessages(logger, reprocessMessage, lstProcessed, lstErrors);

                    //var logInfo = $"{product}:{results.Item1}";
                    //var status = (results.Item2) ? "Error" : "Processed";

                    //// processing has an error
                    //if (results.Item2)
                    //{
                    //    logger.Error(logInfo);
                    //    lstErrors.AddRange(reprocessMessage.ContentSetIds);
                    //}
                    //else
                    //{
                    //    lstProcessed.AddRange(reprocessMessage.ContentSetIds);
                    //    logger.Info(logInfo);
                    //    totalGapFill = totalGapFill + results.Item1;
                    //}

                    //Console.WriteLine($"\n\n{status} - {logInfo}\n\n");
                }
                catch (WebException we)
                {
                    lstErrors.Add($"Error {(int)((HttpWebResponse)we.Response).StatusCode} Processing: {requestUri}");

                    Console.WriteLine($"Error {(int)((HttpWebResponse)we.Response).StatusCode} Processing:\n\n{requestUri}\n\n");
                    _logger.Error(we);
                }
            }

            var execTimeStamp = Helpers.GetDateStampAudit();

            File.WriteAllLines($"Errors-{execTimeStamp}.txt", lstErrors);
            File.WriteAllLines($"Processed-{execTimeStamp}.txt", lstProcessed);

            Console.WriteLine();
            _logger.Info($"Total gaps were filled: {totalGapFill}");

            stopwatch.Stop();

            // Write result.
            _logger.Info($"Time elapsed for GapFill Journals: {stopwatch.Elapsed}");

            if (totalGapFill > 0)
                _logger.Info("Messages were sent");
        }


        public async Task GenerateCSV()
        {
            Helpers.DisplayHeader("Generating CSV");
            Console.ResetColor();

            var storageFiles = new DirectoryInfo(_storagePath);

            foreach (var item in storageFiles.GetFiles().Where(x => x.Extension == ".zip"))
            {
                var contentSetId = Helpers.ExtractContentSetIdFromFile(Path.GetFileNameWithoutExtension(item.Name));

                using (var archive = ZipFile.Open(item.FullName, ZipArchiveMode.Read))
                {

                    var csvFileName = $"{contentSetId}_{archive.Entries.Count}.txt";
                    File.WriteAllText($"{_storagePath}{csvFileName}", "test");
                }

                Console.WriteLine(contentSetId);
            }
        }


        public async Task<List<AssetModel>> HarvestList(string strServiceUri)
        {
            var objMessage = new FillMessage();
            var lstUris = new List<Uri>();
            var lstAssets = new List<AssetModel>();

            try
            {
                var resultObject = await GetHarvestList(strServiceUri);

                var bulkUpdates = resultObject.Products.FirstOrDefault()?.BulkUpdates;
                bulkUpdates.ForEach(
                    bf => bf.BulkFiles.ForEach(
                        harvest => Helpers.ExtractFileInformation(harvest.Location, lstUris, lstAssets)
                        ));

                return lstAssets;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task SomeMajorTask()
        {
            var outputStream = new MemoryStream();

            using (var outputZip = new ZipArchive(outputStream, ZipArchiveMode.Create, leaveOpen: true))
            using (var containerBuilder = new ContainerBuilder(outputZip))
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                CSVMapping csvMapping = null;
                var streamParser = new StreamParser();

                var strFilePath = @"C:\Projects\assetfiles\testfile.zip";

                var fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                var ms = new MemoryStream();
                fs.CopyTo(ms);

                // 
                // var file = _downloader.GetFile(uri);
                //var zip = _zipProcessor.UnZip(file);

                var zip = new ZipArchive(ms, ZipArchiveMode.Read, true);

                foreach (var zipEntry in zip.Entries)
                {
                    if (!zipEntry.FullName.ToLower().EndsWith(".xml")) continue;
                    using (var inputStream = zipEntry.Open())
                    using (var stream = new MemoryStream())
                    {
                        _transformer.Transform(inputStream, stream);
                        if (stream.Length == 0) continue;
                        stream.Position = 0;
                        var data = await streamParser.Parse(stream);

                        csvMapping = _mappingProvider.GetMappingByData(data);
                        var serializer = new CsvSerializer(containerBuilder, csvMapping);

                        await serializer.Serialize(data.ParsedEntities);
                    }
                }

                // Add metadata to each archive
                if (csvMapping != null)
                {
                    CSVMappingHelper.AddMetadata(outputZip, csvMapping);
                }

                stopWatch.Stop();

                var ts = stopWatch.Elapsed;
                var elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            }
        }

        public Task GenerateMessage()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFGRContent(ConfigurationSettings configSettings, string contentType, string productVersion)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFGRContent(System.Configuration.ConfigurationSettings configSettings, string contentType, string productVersion)
        {
            throw new NotImplementedException();
        }

        public async Task<HarvestList> GetHarvestList(string uri)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                requestMessage.Headers.Add("Ocp-Apim-Subscription-Key", _appSettings.ConfigurationSettings.SubscriptionKey);
                requestMessage.Headers.Add("Accept", "application/json; charset=utf-8");

                var response = await _httpClient.SendAsync(requestMessage);
                var responseObject = await response.Content.ReadAsStringAsync();

                // todo: uncomment this for auditing purposes
                // File.WriteAllText($"harvest-audit-{DateTime.Now.ToString("yyyyMMdd-HH-mm-ss")}.json", responseObject);

                return JsonConvert.DeserializeObject<HarvestList>(responseObject);
            }
        }

        public void Initialize(ILoaderOptions options)
        {
            var (validLoaders, isValidLoader) = Helpers.ValidateParameterLoaders(options.Loaders);

            if (!isValidLoader)
                Console.WriteLine("Invalid Loader/s");

            var isValidEnvironment = Helpers.ValidateEnvironment(options.Environment);

            if (!isValidEnvironment)
                Console.WriteLine("Invalid Environment");

            var isValidProducts = (options.Products == "all") ? true : Helpers.ValidateProducts(options.Products);

            if (!isValidProducts)
                Console.WriteLine("Invalid Product Selections");

            Console.WriteLine(" ");
            Helpers.HighlightText("Environment", options.Environment, isValidEnvironment);
            Helpers.HighlightText("Loaders", options.Loaders, isValidLoader);
            Helpers.HighlightText("Version", options.ProductVersion);
            Helpers.HighlightText("Products", options.Products, isValidProducts);
            Console.WriteLine(" ");

            if (isValidLoader && isValidEnvironment && isValidProducts)
            {
                Console.WriteLine($"Reprocessor will run on the above parameters. Do you want to continue? (Y/N) Enter defaults to Y.");

                while (true)
                {
                    ConsoleKeyInfo c = Console.ReadKey(true);
                    if (c.Key == ConsoleKey.Y || c.Key == ConsoleKey.N || c.Key == ConsoleKey.Enter)
                    {
                        if (!(c.Key == ConsoleKey.N))
                        {
                            // user selected y or enter
                            Console.WriteLine("\nProcessing Data Loading...\n");
                            // todo: find ways to avoid this wait()
                            //FillGapsAsync(options.Products.Split(','), options.ProductVersion).Wait();
                            SomeMajorTask();
                        }
                        else
                        {
                            Console.WriteLine("\nProgram Terminated\n");
                        }
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Check red-highlighted items above for errors.");
            }
        }

        public Task SendMessage(LoadRequest loadRequest, string loaderName, bool isSuccess)
        {
            throw new NotImplementedException();
        }

        public void SendMessages(IEnumerable<Uri> assetUrls, string productId, string productVersion)
        {
            throw new NotImplementedException();
        }


    }
}
