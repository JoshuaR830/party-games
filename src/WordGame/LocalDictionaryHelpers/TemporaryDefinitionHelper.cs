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
        
        public void AutomaticallySetTemporaryDefinitionForWord(string filename, string word, string temporaryDefinition)
        {
            var dictionary = _fileHelper.ReadDictionary(filename);
            
            dictionary.Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = null,
                TemporaryDefinition = temporaryDefinition,
                Status = WordStatus.Suffix
            });
            
            _fileHelper.WriteDictionary(filename, dictionary);
        }
    }
}