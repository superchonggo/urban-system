using GapFillUtility.Services.Model;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GapFillUtility.Services.Serialization
{
    public interface IStatementsSerializer
    {
        Task Serialize(string productName, IEnumerable<Entity> entities, Stream outputStream);
    }
}