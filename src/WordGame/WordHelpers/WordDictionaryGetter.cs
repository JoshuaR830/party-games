using System.Collections.Generic;
using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public class WordDictionaryGetter
    {
        public static readonly Dictionary<string, WordDictionary> WordDictionary = new Dictionary<string, WordDictionary>();
        public static readonly Dictionary<string, string> StringDictionary = new Dictionary<string, string>();

        public static void AddToDictionary(string filename, WordDictionary data)
        {
            WordDictionary.Add(filename, data);
        }
        
        public static void AddToDictionary(string filename, string data)
        {
            StringDictionary.Add(filename, data);
        }
    }
}