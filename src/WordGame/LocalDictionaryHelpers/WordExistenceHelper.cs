using System.Linq;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class WordExistenceHelper : IWordExistenceHelper
    {
        private readonly IFileHelper _fileHelper;
        private const string Filename = "./word-list.json";

        public WordExistenceHelper(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public bool DoesWordExist(string word)
        {
            var dictionary = _fileHelper.ReadDictionary(Filename);
            return dictionary
                .Words
                .Where(x => x.Word.ToLower() == word.ToLower())
                .ToList()
                .Any();
        }
    }
}