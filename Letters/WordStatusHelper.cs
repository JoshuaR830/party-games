using System.Collections.Generic;
using System.Linq;

namespace Chat.Letters
{
    public class WordStatusHelper
    {
        public List<WordAcceptability> Words { get; private set; }

        public WordStatusHelper()
        {
            this.Words = new List<WordAcceptability>();
        }

        public bool GetWordStatus(string word)
        {
            if (ContainsWord(word))
            {
                var selectedWord = this.Words.Where(x => x.Word == word).First();
                return selectedWord.IsAcceptable;
            }

            return FindUnknownWordStatus(word);
        }

        public bool ContainsWord(string word)
        {
            var fileHelper = new FileHelper();
            this.Words = fileHelper.ReadWordsFromFile();
            System.Console.WriteLine(this.Words);
            return this.Words.Any(x => x.Word == word);
        }

        public bool FindUnknownWordStatus(string word)
        {
            if(ContainsWord(word))
                return false;

            var validationHelper = new WordValidationHelper();
            var isValidWord = validationHelper.MakeWebRequest(word);
            this.Words.Add(new WordAcceptability(word, isValidWord));

            var fileHelper = new FileHelper();
            fileHelper.SaveWordsToFile(this.Words);

            return isValidWord;
        }
    }
}