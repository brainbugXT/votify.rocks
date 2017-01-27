using System.IO;

namespace Votify.Rocks.Service
{
    public interface IResourceTextReader
    {
        string ReadString(string resourceName, string ext);
        MemoryStream ReadMemoryStream(string resourceName, string ext);
    }
}
