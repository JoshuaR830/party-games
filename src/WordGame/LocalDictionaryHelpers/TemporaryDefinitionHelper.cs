using Chat.WordGame.WordHelpers;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class TemporaryDefinitionHelper : ITemporaryDefinitionHelper
    {
        readonly IFileHelper _fileHelper;

        public TemporaryDefinitionHelper(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }
        
        public void AutomaticallySetTemporaryDefinitionForWord(Dictionary dictionary, string word, string temporaryDefinition)
        {
            dictionary.Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = null,
                TemporaryDefinition = temporaryDefinition,
                Status = WordStatus.Suffix
            });
        }
    }
}