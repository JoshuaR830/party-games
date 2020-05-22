namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class FilenameHelper : IFilenameHelper
    {
        public string Filename;

        public FilenameHelper()
        {
            Filename = "./word-dictionary.json";
        }
        
        public void SetDictionaryFilename(string fileName)
        {
            Filename = fileName;
        }

        public string GetDictionaryFilename()
        {
             return Filename;
        }
    }
}