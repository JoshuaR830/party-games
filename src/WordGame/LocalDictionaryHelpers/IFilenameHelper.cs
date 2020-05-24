namespace Chat.WordGame.LocalDictionaryHelpers
{
    public interface IFilenameHelper
    {
        void SetDictionaryFilename(string fileName);
        string GetDictionaryFilename();
        void SetGuessedWordsFilename(string filename);
        string GetGuessedWordsFilename();
    }
}