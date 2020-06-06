using System.Collections.Generic;
using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public class WordDictionaryGetter
    {
        // private static WordDictionaryGetter _myWordDictionary;

        public static readonly Dictionary<string, WordDictionary> WordDictionary = new Dictionary<string, WordDictionary>();
        public static readonly Dictionary<string, string> StringDictionary = new Dictionary<string, string>();

        // private WordDictionaryGetter()
        // {
        //     
        // }
        //
        // public static WordDictionaryGetter GetWordDictionaryGetter()
        // {
        //     if (_myWordDictionary == null)
        //     {
        //         _myWordDictionary = new WordDictionaryGetter();
        //     }
        //
        //     return _myWordDictionary;
        // }

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