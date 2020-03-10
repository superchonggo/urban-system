using System.IO;

namespace GapFillUtility.Services.Interface
{
    public interface ITransformer
    {
        void Transform(Stream inStream, Stream outStream);
    }
}
