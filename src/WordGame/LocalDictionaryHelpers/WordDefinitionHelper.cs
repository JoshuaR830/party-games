namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class WordDefinitionHelper : IWordDefinitionHelper
    {
        private readonly IFileHelper _fileHelper;

        public WordDefinitionHelper(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public string GetDefinitionForWord(string word)
        {
     
        }
    }
}