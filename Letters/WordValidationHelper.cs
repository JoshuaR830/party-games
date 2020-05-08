using System.Net;

namespace Chat.Letters {
    public class WordValidationHelper
    {
        public WordStatusHelper wordStatusHelper { get; }
        public WordValidationHelper() {
            this.wordStatusHelper = new WordStatusHelper();
        }
        // ToDo: Need to be singleton so that it doesn't keep building this up
        public bool ValidateWord(string word)
        {
            return this.wordStatusHelper.GetWordStatus(word);
        }

        public string GetDefinition(string word)
        {
            return this.wordStatusHelper.GetDefinition(word);
        }

        public void UpdateDictionary(string newWord, string newDefinition){
            this.wordStatusHelper.UpdateDictionary(newWord, newDefinition);
        }
    }
}