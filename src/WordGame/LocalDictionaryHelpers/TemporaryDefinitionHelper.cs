using Chat.WordGame.WordHelpers;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class TemporaryDefinitionHelper : ITemporaryDefinitionHelper
    {
        readonly IFileHelper _fileHelper;
        readonly IFilenameHelper _filenameHelper;
        public TemporaryDefinitionHelper(IFileHelper fileHelper, IFilenameHelper filenameHelper)
        {
            _fileHelper = fileHelper;
            _filenameHelper = filenameHelper;
        }
        
        public void AutomaticallySetTemporaryDefinitionForWord(WordDictionary wordDictionary, string word, string temporaryDefinition)
        {
            _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename()).Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = null,
                TemporaryDefinition = temporaryDefinition,
                Status = WordStatus.Suffix
            });
        }
    }
}