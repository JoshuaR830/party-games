using System;
using System.Linq;
using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public class WordService : IWordService
    {
        private readonly IWordHelper _wordHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IFileHelper _fileHelper;
        
        public WordService(IWordExistenceHelper wordExistenceHelper, IWordHelper wordHelper, IWordDefinitionHelper wordDefinitionHelper, IFileHelper fileHelper)
        {
            _wordExistenceHelper = wordExistenceHelper;
            _wordHelper = wordHelper;
            _wordDefinitionHelper = wordDefinitionHelper;
            _fileHelper = fileHelper;
        }

        public bool GetWordStatus(string filename, string word)
        {
            var wordExists = _wordExistenceHelper.DoesWordExist(word);
            
            if (wordExists)
                return true;
            
            wordExists = _wordHelper.StrippedSuffixDictionaryCheck(filename, word);

            return wordExists;
        }

        public string GetDefinition(string filename, string word)
        {
            if (GetWordStatus(filename, word))
                return _wordDefinitionHelper.GetDefinitionForWord(word);

            return null;
        }

        public void AddNewWordToDictionary(string filename, string word, string definition)
        {
            var dictionary = _fileHelper.ReadDictionary(filename);
            
            dictionary.Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = definition,
                TemporaryDefinition = null
            });
            
            _fileHelper.WriteDictionary(filename, dictionary);
        }

        public void UpdateExistingWord(string filename, string word, string definition)
        {
            if (word == "" || definition == "")
                return;
            
            var dictionary = _fileHelper.ReadDictionary(filename);

            var wordList = dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!wordList.Any())
                return;

            var item = dictionary.Words.IndexOf(wordList.First());
            dictionary.Words[item].PermanentDefinition = definition;
            
            _fileHelper.WriteDictionary(filename, dictionary);
        }

        public void AutomaticallySetTemporaryDefinitionForWord(string filename, string word, string temporaryDefinition)
        {
            var dictionary = _fileHelper.ReadDictionary(filename);
            
            dictionary.Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = null,
                TemporaryDefinition = temporaryDefinition
            });
            
            _fileHelper.WriteDictionary(filename, dictionary);
        }
    }
}