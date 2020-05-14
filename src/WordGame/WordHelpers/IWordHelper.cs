namespace Chat.WordGame.WordHelpers
{
    public interface IWordHelper
    {
        bool StrippedSuffixDictionaryCheck(string word);
        bool CheckWordWithEndingExists(string word, string shortWord);
    }
}