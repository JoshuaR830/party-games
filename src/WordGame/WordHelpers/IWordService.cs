using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public interface IWordService
    {
        bool GetWordStatus(string word);
        string GetDefinition(string word);
        void AddNewWordToDictionary(string filename, string word, string definition);
        void UpdateExistingWord(string filename, string word, string definition);
    }
}