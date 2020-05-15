using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public interface IWordService
    {
        bool GetWordStatus(string filename, string word);
        string GetDefinition(string filename, string word);
        void AddNewWordToDictionary(string filename, string word, string definition);
        void UpdateExistingWord(string filename, string word, string definition);
        void AutomaticallySetTemporaryDefinitionForWord(string filename, string word, string temporaryDefinition);
    }
}