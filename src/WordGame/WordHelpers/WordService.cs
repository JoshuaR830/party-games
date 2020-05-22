using System;
using System.Linq;
using Chat.WordGame.LocalDictionaryHelpers;
using Newtonsoft.Json;

namespace Chat.WordGame.WordHelpers
{
    public class WordService : IWordService
    {
        private readonly IWordHelper _wordHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IFileHelper _fileHelper;
        private readonly IFilenameHelper _filenameHelper;

        private readonly Dictionary _dictionary;
        
        public WordService(IWordExistenceHelper wordExistenceHelper, IWordHelper wordHelper, IWordDefinitionHelper wordDefinitionHelper, IFileHelper fileHelper, IFilenameHelper filenameHelper)
        {
            _wordExistenceHelper = wordExistenceHelper;
            _wordHelper = wordHelper;
            _wordDefinitionHelper = wordDefinitionHelper;
            _fileHelper = fileHelper;
            _filenameHelper = filenameHelper;

            if (_dictionary == null)
            {
                Console.WriteLine("Hello");
                var name = _filenameHelper.GetDictionaryFilename();
                _dictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
            }
        }

        public bool GetWordStatus(string filename, string word)
        {
            var wordExists = _wordExistenceHelper.DoesWordExist(word);
            
            if (wordExists)
                return true;
            
            wordExists = _wordHelper.StrippedSuffixDictionaryCheck(filename, word);

            return wordExists;
        }

        public string GetDefinition(string filename, string word)
        {
            if (GetWordStatus(filename, word))
                return _wordDefinitionHelper.GetDefinitionForWord(word);

            return null;
        }

        public void AmendDictionary(string filename, string word, string definition)
        {

            if (_dictionary.Words.Any(x => x.Word.ToLower() == word.ToLower()))
            {
                UpdateExistingWord(filename, word, definition);
            }
            else
            {
                AddNewWordToDictionary(filename, word, definition);
            }
            
        }

        public void AddNewWordToDictionary(string filename, string word, string definition)
        {
            _dictionary.Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = definition,
                TemporaryDefinition = null,
                Status = WordStatus.Permanent
            });
            
            _fileHelper.WriteDictionary(filename, _dictionary);
        }

        public void UpdateExistingWord(string filename, string word, string definition)
        {
            if (word == "" || definition == "")
                return;
            
            var wordList = _dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!wordList.Any())
                return;

            var item = _dictionary.Words.IndexOf(wordList.First());
            _dictionary.Words[item].PermanentDefinition = definition;
            _dictionary.Words[item].Status = WordStatus.Permanent;

            _fileHelper.WriteDictionary(filename, _dictionary);
        }

        public void ToggleIsWordInDictionary(string filename, string word, bool expectedNewStatus)
        {
            var items = _dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!items.Any())
                return;

            var item = items.First();

            var index = _dictionary.Words.IndexOf(item);

            if (expectedNewStatus == false)
            {
                _dictionary.Words[index].Status = WordStatus.DoesNotExist;
                _fileHelper.WriteDictionary(filename, _dictionary);
                return;
            }
            
            Console.WriteLine(JsonConvert.SerializeObject(_dictionary.Words[index]));
            _dictionary.Words[index].Status = item.PermanentDefinition != null ? WordStatus.Permanent : WordStatus.Temporary;
            Console.WriteLine(JsonConvert.SerializeObject(_dictionary.Words[index]));
            
            _fileHelper.WriteDictionary(filename, _dictionary);
        }

        public void AddWordToGuessedWords(string dictionaryFilename, string guessedWordsFilename, string word)
        {
            var items = _dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            var guessedWordsJson = _fileHelper.ReadFile(guessedWordsFilename);
            var guessedWords = JsonConvert.DeserializeObject<GuessedWords>(guessedWordsJson) ?? new GuessedWords();
            var guessed = guessedWords.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
            
            if (guessed.Any())
            {
                var index = guessedWords.Words.IndexOf(guessed.First());
                guessedWords.Words[index] = new GuessedWord(word, items.Any() ? items.First().Status : WordStatus.DoesNotExist);
            }
            else
            {
                guessedWords.AddWord(word, items.Any() ? items.First().Status : WordStatus.DoesNotExist);
            }

            _fileHelper.WriteFile(guessedWordsFilename, guessedWords);
        }
    }
}