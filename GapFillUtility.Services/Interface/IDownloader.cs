using System;
using System.IO;

namespace GapFillUtility.Services.Interface
{
    public interface IDownloader
    {
        int MaxRetries { get; set; }
        Stream GetFile(Uri url);
    }
}
