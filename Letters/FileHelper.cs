
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Chat.Letters
{
    public class FileHelper
    {
        public void SaveWordsToFile(List<WordAcceptability> words)
        {
            TextWriter writer = null;
            try
            {
                var json = JsonConvert.SerializeObject(words);
                writer = new StreamWriter("./word-list.json", false);
                writer.Write(json);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public List<WordAcceptability> ReadWordsFromFile()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader("./word-list.json");
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<WordAcceptability>>(fileContent);
            }
            finally
            {
                if (reader != null)        
                    reader.Close();
            }
        }
    }
}