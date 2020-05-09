namespace Chat.WordGame.WordHelpers
{
    public interface IWordHelper
    {
        bool StrippedSuffixDictionaryCheck(string word);
        bool CheckWordEndingExists(string word);
    }
}