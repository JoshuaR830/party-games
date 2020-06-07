using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chat.WordGame.WordHelpers;
using Newtonsoft.Json;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class FileHelper : IFileHelper
    {
        private readonly IFilenameHelper _filenameHelper;
        
        public FileHelper(IFilenameHelper filenameHelper)
        {
            _filenameHelper = filenameHelper;
        }
        
        public WordDictionary ReadDictionary(string filename)
        {
            if (WordDictionaryGetter.WordDictionary.Any(x => x.Key == filename))
            {
                Console.WriteLine("Dictionary accessed but not read from file");
                return WordDictionaryGetter.WordDictionary[_filenameHelper.GetDictionaryFilename()];
            }
            
            TextReader reader = null;
            try
            {
                Console.WriteLine($"Read {filename}");
                reader = new StreamReader(filename);
                var fileContent = reader.ReadToEnd();
                var data = JsonConvert.DeserializeObject<WordDictionary>(fileContent);
                WordDictionaryGetter.AddToDictionary(filename, data);

                return data;
            }
            finally
            {
                reader?.Close();
            }
        }

        public void WriteDictionary(string filename, WordDictionary wordDictionary)
        {
            WriteFile(filename, wordDictionary);
        }

        public string ReadFile(string filename)
        {
            if (WordDictionaryGetter.StringDictionary.Any(x => x.Key == filename))
            {
                Console.WriteLine("Generic file accessed but not read from file");
                return WordDictionaryGetter.StringDictionary.First(x => x.Key == filename).Value;
            }

            TextReader reader = null;
            try
            {
                Console.WriteLine("Read generic file");
                reader = new StreamReader(filename);
                var fileContent = reader.ReadToEnd();
                WordDictionaryGetter.AddToDictionary(filename, fileContent);
                return fileContent;
            }
            finally
            {
                reader?.Close();
            }
        }

        public void WriteFile(string filename, object data)
        {
            var json = JsonConvert.SerializeObject(data);

            try
            {
                File.WriteAllText(filename, json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred {e}");
            }
        }
    }
}