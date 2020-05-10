using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public class WordService : IWordService
    {
        private readonly IWordHelper _wordHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordDefinitionHelper _wordDefinitionHelper;

        public WordService(IWordExistenceHelper wordExistenceHelper, IWordHelper wordHelper, IWordDefinitionHelper wordDefinitionHelper)
        {
            _wordExistenceHelper = wordExistenceHelper;
            _wordHelper = wordHelper;
            _wordDefinitionHelper = wordDefinitionHelper;
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
            throw new System.NotImplementedException();
        }
    }
}