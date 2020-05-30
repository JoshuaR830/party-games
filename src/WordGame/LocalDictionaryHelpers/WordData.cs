using System.Collections.Generic;
using Chat.WordGame.WordHelpers;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class WordData
    {
        public string Word { get; set; }
        public string TemporaryDefinition { get; set; }
        public string PermanentDefinition { get; set; }
        public WordStatus Status { get; set; }
        public WordCategory Category { get; set; }
    }

    public class Dictionary
    {
        public List<WordData> Words { get; set; }

        public Dictionary()
        {
            Words = new List<WordData>();
        }
    }
}