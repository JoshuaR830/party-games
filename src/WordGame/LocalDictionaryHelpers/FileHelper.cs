using System.IO;
using Newtonsoft.Json;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class FileHelper : IFileHelper
    {
        public Dictionary ReadDictionary(string filename)
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filename);
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary>(fileContent);
            }
            finally
            {
                reader?.Close();
            }
        }
    }
}