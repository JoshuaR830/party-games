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
            dictionary
                .Words
                .Where(x => x.Word.ToLower() == word.ToLower())
                .ToList()
                .Any();
            
            var definitionParts = dictionary
                .Words[0]
                .PermanentDefinition
                .Split(new Char[] {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '\n'})
                .Where(y => !string.IsNullOrWhiteSpace(y))
                .ToList();

            return definitionParts
                .Where(x => (!x.ToLower().Contains("obs.") && !x.ToLower().Contains("archaic") && !x.ToLower().Contains("scot.") && !x.ToLower().Contains("[irish]")))
                .ToList()
                .Any();
            // Make sure to include ;
        }
    }
}