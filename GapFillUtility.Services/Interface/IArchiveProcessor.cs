using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Interface
{
    public interface IArchiveProcessor
    {
        Task<IEnumerable<Tuple<Stream, string>>> Process(ZipArchive archive);
    }
}
