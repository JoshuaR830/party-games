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
                if (word.Substring(word.Length - ending.Length) != ending)
                    continue;

                if (!_wordExistenceHelper.DoesWordExist(word.Remove(word.Length - ending.Length))) 
                    continue;

                if (CheckWordWithEndingExists(word, word.Remove(word.Length - ending.Length)))
                    return true;
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