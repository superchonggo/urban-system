using GapFillUtility.Services.Interface;
using System.IO;
using System.IO.Compression;

namespace GapFillUtility.Services.Processors
{
    public class HarvestZipProcessor : IZipProcessor
    {
        public ZipArchive UnZip(Stream stream)
        {
            return new ZipArchive(stream, ZipArchiveMode.Read, true);
        }
    }
}
