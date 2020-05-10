using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public class WordService : IWordService
    {
        private readonly IWordHelper _wordHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;

        public WordService(IWordExistenceHelper wordExistenceHelper, IWordHelper wordHelper)
        {
            _wordExistenceHelper = wordExistenceHelper;
            _wordHelper = wordHelper;
        }

        public bool GetWordStatus(string word)
        {
            var wordExists = _wordExistenceHelper.DoesWordExist(word);
            
            if (wordExists)
                return true;
            
            wordExists = _wordHelper.StrippedSuffixDictionaryCheck(word);

            return wordExists;
        }
    }
}