
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System;
using Chat.WordGame.LocalDictionaryHelpers;

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

        public List<WordStuff> ReadWordsFromFile()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader("./new-word-list.json");
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<WordStuff>>(fileContent);
            }
            finally
            {
                if (reader != null)        
                    reader.Close();
            }
        }

        public List<string> ReadDictionary()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader("./dictionary.json");
                var fileContent = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<string>>(fileContent);
            }
            finally
            {
                if (reader != null)        
                    reader.Close();
            }
        }

        public void WriteToDictionary(List<string> words)
        {
            Console.WriteLine("Writing to dictionary");
            var dictionaryHelper = new DictionaryHelper();

            foreach(var word in words)
            {
                var split = word.Split("###");
                var thing = new WordStuff(split[0], split[1]);
                dictionaryHelper.AppendWord(thing);
            }

            var sortedDictionary = dictionaryHelper.Dictionary.OrderBy(x => x.Word).ToList();

            TextWriter writer = null;
            try
            {
                var json = JsonConvert.SerializeObject(sortedDictionary);
                writer = new StreamWriter("./new-word-list.json", false);
                writer.Write(json);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public class DictionaryHelper
        {
            public List<WordStuff> Dictionary {get; private set;}

            public DictionaryHelper() {
                this.Dictionary = new List<WordStuff>();
            }

            public void AppendWord(WordStuff stuff)
            {
                this.Dictionary.Add(stuff);
            }
        }

        public class WordStuff
        {
            public string Word { get; }
            public string Definition { get; private set; }

            public WordStuff(string word, string definition)
            {
                this.Word = word;
                this.Definition = definition;
            }

            public void ChangeDefinition(string newDefintion)
            {
                this.Definition = newDefintion;
            }
        }

        public void SaveNewWordToFile(List<WordStuff> words, string newWord, string newDefinition)
        {
            
            var wordStatusHelper = new WordStatusHelper();
            if (wordStatusHelper.ContainsWord(newWord))
            {
                wordStatusHelper.Words.First(x => x.Word == newWord).ChangeDefinition(newDefinition);
            } else {
                words.Add(new WordStuff(newWord.ToLower(), newDefinition));
            }

            TextWriter writer = null;
            try
            {
                var json = JsonConvert.SerializeObject(words);
                writer = new StreamWriter("./new-word-list.json", false);
                writer.Write(json);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
        
        public void CopyFileContent()
        {
            Console.WriteLine("Started");
            var helper = new WordGame.LocalDictionaryHelpers.FileHelper();
            var list = ReadWordsFromFile();

            var dictionary = new WordGame.LocalDictionaryHelpers.Dictionary();
            
            foreach (var item in list)
            {
                dictionary.Words.Add(new WordData
                {
                    Word = item.Word,
                    TemporaryDefinition = item.Definition,
                    PermanentDefinition = null
                });
            }

            Console.WriteLine("Done");
            
            helper.WriteDictionary("./word-dictionary.json", dictionary);
        }
    }
}