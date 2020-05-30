namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class FilenameHelper : IFilenameHelper
    {
        public string DictionaryFilename;
        public string GuessedWordsFilename;

        public FilenameHelper()
        {
            DictionaryFilename = "./word-dictionary.json";
            GuessedWordsFilename = "./words-guessed.json";
        }
        
        public void SetDictionaryFilename(string fileName)
        {
            DictionaryFilename = fileName;
        }

        public string GetDictionaryFilename()
        {
             return DictionaryFilename;
        }

        public void SetGuessedWordsFilename(string filename)
        {
            GuessedWordsFilename = filename;
        }

        public string GetGuessedWordsFilename()
        {
            return GuessedWordsFilename;
        }
    }
}