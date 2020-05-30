using System;
using System.IO;
using System.Text;
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
                Console.WriteLine($"Read {filename}");
                reader = new StreamReader(filename);
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary>(fileContent);
            }
            finally
            {
                reader?.Close();
            }
        }

        public void WriteDictionary(string filename, Dictionary dictionary)
        {
            WriteFile(filename, dictionary);
        }

        public string ReadFile(string filename)
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filename);
                var fileContent = reader.ReadToEnd();
                return fileContent;
            }
            finally
            {
                reader?.Close();
            }
        }

        public void WriteFile(string filename, object data)
        {
            // if (File.Exists(filename))
            //     File.Delete(filename);

            Console.WriteLine(data);
            
            var json = JsonConvert.SerializeObject(data);

            try
            {
                File.WriteAllText(filename, json);
                // using (var writer = new StreamWriter(filename, false))
                // {
                //     var content = new UTF8Encoding(true).GetBytes(json);
                //     writer.Write(content);
                // }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred {e}");
            }
        }
    }
}