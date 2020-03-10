using System;
using System.IO;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Serialization
{
    public interface IContainerBuilder: IDisposable
    {
        Task WriteHeader(string entityId, Func<TextWriter, Task> headersWriter);
        Task WriteContent(string entityId, Func<TextWriter, Task> contentWriter);
    }
}