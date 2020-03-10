using System.IO;
using System.IO.Compression;

namespace GapFillUtility.Services.Interface
{
    public interface IZipProcessor
    {
        ZipArchive UnZip(Stream stream);
    }
}
