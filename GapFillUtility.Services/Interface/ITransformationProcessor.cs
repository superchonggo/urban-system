using System;
using System.IO;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Interface
{
    public interface ITransformationProcessor
    {
        Task ProcessAsync(Uri uri, Stream outputStream);
    }
}
