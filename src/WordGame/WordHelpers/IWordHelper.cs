using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.WordGame.WordHelpers
{
    public interface IWordHelper
    {
        bool StrippedSuffixDictionaryCheck(Dictionary dictionary, string word);
        bool CheckWordWithEndingExists(string word, string shortWord);
    }
}