using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public interface IWordHelper
    {
        bool StrippedSuffixDictionaryCheck(WordDictionary wordDictionary, string word);
        bool CheckWordWithEndingExists(string word, string shortWord);
    }
}