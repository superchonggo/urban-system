using GapFillUtility.Services.Interface;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Processors
{
    public class HarvestDownloader : IDownloader
    {
        public int MaxRetries { get; set; } = 3;

        public Stream GetFile(Uri url)
        {
            var tries = 0;
            Stream stream = null;
            do
            {
                try
                {
                    tries++;
                    stream = OpenStream(url);
                }
                catch (Exception)
                {
                    Console.WriteLine("net_retry");
                    Task.Delay((tries + 1) * 300).Wait();
                }
            } while (stream == null && tries < MaxRetries);

            if (stream == null) throw new WebException(string.Format("Cannot reach URI: {0}", url));

            return stream;
        }

        private Stream OpenStream(Uri url)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
