using System.Collections.Generic;
using System.Linq;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;
using Microsoft.CodeAnalysis;

namespace Chat.WordGame.WordHelpers
{
    public class WordHelper : IWordHelper
    {
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly ITemporaryDefinitionHelper _temporaryDefinitionHelper;

        public WordHelper(IWebDictionaryRequestHelper webDictionaryRequestHelper, IWordExistenceHelper wordExistenceHelper, IWordDefinitionHelper wordDefinitionHelper, ITemporaryDefinitionHelper temporaryDefinitionHelper)
        {
            _webDictionaryRequestHelper = webDictionaryRequestHelper;
            _wordExistenceHelper = wordExistenceHelper;
            _wordDefinitionHelper = wordDefinitionHelper;
            _temporaryDefinitionHelper = temporaryDefinitionHelper;
        }

        public bool StrippedSuffixDictionaryCheck(WordDictionary wordDictionary, string word)
        {
            word = word.ToLower();
            var endings = new List<string> {"ning", "ing", "ed", "er", "es", "s", "d"};
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
                    var temporaryDefinition = _wordDefinitionHelper.GetDefinitionForWord(shortenedWord);
                    _temporaryDefinitionHelper.AutomaticallySetTemporaryDefinitionForWord(wordDictionary, word, temporaryDefinition);
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