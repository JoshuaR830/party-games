using System;
using System.Linq;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class WordExistenceHelper : IWordExistenceHelper
    {
        private readonly IFileHelper _fileHelper;
        private const string Filename = "./word-list.json";

        public WordExistenceHelper(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public bool DoesWordExist(string word)
        {
            var dictionary = _fileHelper.ReadDictionary(Filename);
            var matchingWords = dictionary
                .Words
                .Where(x => x.Word.ToLower() == word.ToLower())
                .ToList();

            if (!matchingWords.Any())
                return false;
            
            var selectedWord = matchingWords.First();

            var definitionParts = selectedWord
                .PermanentDefinition
                .Split(new Char[] {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '\n', ';'})
                .Where(y => !string.IsNullOrWhiteSpace(y))
                .ToList();

            return definitionParts
                .Where(x => (!x.ToLower().Contains("obs.") && !x.ToLower().Contains("archaic") && !x.ToLower().Contains("scot.") && !x.ToLower().Contains("[irish]")))
                .ToList()
                .Any();
        }
    }
}