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
            // This is mocked out so that's a lot of fun
            // I want to test the real one
            // I want to read fake dictionary - well I want to actually do it
            // I want to add to write real data to the file
            // Not really sure how to fix this
            var dictionary = _fileHelper.ReadDictionary(filename);
            _fileHelper.WriteDictionary(filename, dictionary);
        }
    }
}