using System.IO;
using System.Reflection;

namespace Votify.Rocks.Service
{
    public class EmailTemplateResourceReader : IResourceTextReader
    {
        public const string NameSpace = "Votify.Rocks.Service";
        //Votify.Rocks.Service.MailTemplates.VoteSessionResults.html
        public EmailTemplateResourceReader()
        {
        }

        public string ReadString(string resourceName, string ext)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceFullName = $"{NameSpace}.MailTemplates.{resourceName}.{ext}";
            string output = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
            {
                if (stream != null)
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        output = reader.ReadToEnd();
                    }
            }

            return output;
        }

        public MemoryStream ReadMemoryStream(string resourceName, string ext)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceFullName = $"{NameSpace}.MailTemplates.{resourceName}.{ext}";
            using (Stream stream = assembly. GetManifestResourceStream(resourceFullName))
            {
                if (stream == null) return null;

                byte[] ba = new byte[stream.Length];
                var ms = new MemoryStream();
                ms.Write(ba, 0, ba.Length);
                return ms;//new MemoryStream(stream.Read(ba, 0, ba.Length));
            }

            
        }
    }
}
