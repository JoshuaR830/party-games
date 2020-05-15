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

        public bool GetWordStatus(string word)
        {
            var wordExists = _wordExistenceHelper.DoesWordExist(word);
            
            if (wordExists)
                return true;
            
            wordExists = _wordHelper.StrippedSuffixDictionaryCheck(word);

            return wordExists;
        }

        public string GetDefinition(string word)
        {
            if (GetWordStatus(word))
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
    }
}