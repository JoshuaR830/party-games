using System.Linq;

namespace Chat.WordGame.LocalDictionaryHelpers
{
    public class WordDefinitionHelper : IWordDefinitionHelper
    {
        private readonly IFileHelper _fileHelper;
        private const string Filename = "./word-dictionary.json";

        public WordDefinitionHelper(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public string GetDefinitionForWord(string word)
        {
            var words = _fileHelper.ReadDictionary(Filename)
                .Words
                .Where(x => x.Word.ToLower() == word.ToLower())
                .ToList();

            if (!words.Any())
                return "";

            return (words.First().PermanentDefinition ?? words.First().TemporaryDefinition) ?? "";
        }
    }
}