using System.Collections.Generic;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class WordData
    {
        public string Word { get; set; }
        public string TemporaryDefinition { get; set; }
        public string PermanentDefinition { get; set; }
    }

    public class Dictionary
    {
        public List<WordData> Words { get; set; }

        public Dictionary()
        {
            this.Words = new List<WordData>();
        }
    }
}