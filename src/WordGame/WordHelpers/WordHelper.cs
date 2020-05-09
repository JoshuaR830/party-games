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
            var endings = new List<string> {"ning", "ing", "er", "es", "s"};
            endings = endings
                .Where(x => x.Length < word.Length)
                .OrderByDescending(s => s.Length)
                .ToList();

            foreach (var ending in endings)
            {
                if (word.Substring(word.Length - ending.Length) != ending)
                    continue;

                if (!_wordExistenceHelper.DoesWordExist(word.Remove(word.Length - endings.Count))) 
                    continue;

                if (CheckWordEndingExists(word))
                    return true;
            }

            return false;
        }

        public bool CheckWordEndingExists(string word)
        {
            var responseText = _webDictionaryRequestHelper.MakeContentRequest(word);
            return responseText.Contains(word);
        }
    }
}