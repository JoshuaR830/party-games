using System.Collections.Generic;
using System.Linq;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;

namespace Chat.WordGame.WordHelpers
{
    public class WordHelper : IWordHelper
    {
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;

        public WordHelper(IWebDictionaryRequestHelper webDictionaryRequestHelper, IWordExistenceHelper wordExistenceHelper)
        {
            _webDictionaryRequestHelper = webDictionaryRequestHelper;
            _wordExistenceHelper = wordExistenceHelper;
        }

        public bool StrippedSuffixDictionaryCheck(string word)
        {
            var endings = new List<string> {"ning", "ing", "ed", "er", "es", "s"};
            endings = endings
                .Where(x => x.Length < word.Length)
                .OrderByDescending(s => s.Length)
                .ToList();


            foreach (var ending in endings)
            {
                var shortenedWord = word.Remove(word.Length - ending.Length);

                if (word.Substring(word.Length - ending.Length) != ending)
                    continue;

                if (!_wordExistenceHelper.DoesWordExist(shortenedWord)) 
                    continue;

                if (CheckWordWithEndingExists(word, shortenedWord))
                {
                    // Need to do whatever I do here
                    // Need to get the definition of the short word
                    // Need to set that as temporary definition
                    // Need to set the full word as the word

                    // Need to write a test that will check that plural is added
                    // Need to substitute everything except the stripped suffix stuff
                    return true;
                }
            }

            return false;
        }

        public bool CheckWordWithEndingExists(string word, string shortWord)
        {
            var responseText = _webDictionaryRequestHelper.MakeContentRequest(shortWord).ToLower();
            if (!responseText.Contains("word forms"))
                return false;
            return responseText.Contains(word);
        }
    }
}