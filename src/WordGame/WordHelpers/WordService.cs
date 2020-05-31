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
        private readonly GuessedWords _guessedWords;

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
                _dictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
            }

            if (_guessedWords == null)
            {
                Console.WriteLine("Guessed words");
                var json = _fileHelper.ReadFile(_filenameHelper.GetGuessedWordsFilename());
                _guessedWords = JsonConvert.DeserializeObject<GuessedWords>(json) ?? new GuessedWords();

            }
        }

        public bool GetWordStatus(string filename, string word)
        {
            var wordExists = _wordExistenceHelper.DoesWordExist(word);
            
            if (wordExists)
                return true;
            
            wordExists = _wordHelper.StrippedSuffixDictionaryCheck(_dictionary, word);

            return wordExists;
        }

        public string GetDefinition(string filename, string word)
        {
            if (GetWordStatus(filename, word))
                return _wordDefinitionHelper.GetDefinitionForWord(word);

            return null;
        }
        
        public WordCategory GetCategory(string filename, string word)
        {
            if (GetWordStatus(filename, word))
                return _dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList()[0].Category;

            return WordCategory.None;
        }

        public void AmendDictionary(string filename, string word, string definition, WordCategory category = WordCategory.None)
        {

            if (_dictionary.Words.Any(x => x.Word.ToLower() == word.ToLower()))
            {
                UpdateExistingWord(filename, word, definition, category);
            }
            else
            {
                AddNewWordToDictionary(filename, word, definition, category);
            }
            
        }

        public void AddNewWordToDictionary(string filename, string word, string definition, WordCategory category = WordCategory.None)
        {
            _dictionary.Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = definition,
                TemporaryDefinition = null,
                Status = WordStatus.Permanent,
                Category = category
            });
        }

        public void UpdateExistingWord(string filename, string word, string definition, WordCategory category = WordCategory.None)
        {
            if (word == "" || definition == "")
                return;
            
            var wordList = _dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!wordList.Any())
                return;

            var item = _dictionary.Words.IndexOf(wordList.First());
            _dictionary.Words[item].PermanentDefinition = definition;
            _dictionary.Words[item].Status = WordStatus.Permanent;
            _dictionary.Words[item].Category = category;
        }

        public void UpdateCategory(string filename, string word, WordCategory category)
        {
            var wordList = _dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!wordList.Any())
                return;

            var item = _dictionary.Words.IndexOf(wordList.First());
            _dictionary.Words[item].Category = category;
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
                return;
            }
            
            Console.WriteLine(JsonConvert.SerializeObject(_dictionary.Words[index]));
            _dictionary.Words[index].Status = item.PermanentDefinition != null ? WordStatus.Permanent : WordStatus.Temporary;
            Console.WriteLine(JsonConvert.SerializeObject(_dictionary.Words[index]));
        }

        public void AddWordToGuessedWords(string dictionaryFilename, string guessedWordsFilename, string word)
        {
            var items = _dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
            
            var guessed = _guessedWords.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
            
            if (guessed.Any())
            {
                var index = _guessedWords.Words.IndexOf(guessed.First());
                _guessedWords.Words[index] = new GuessedWord(word, items.Any() ? items.First().Status : WordStatus.DoesNotExist);
            }
            else
            {
                _guessedWords.AddWord(word, items.Any() ? items.First().Status : WordStatus.DoesNotExist);
            }

        }
        
        public void UpdateDictionaryFile()
        {
            _fileHelper.WriteFile(_filenameHelper.GetDictionaryFilename(), _dictionary);
        }

        public void UpdateGuessedWordsFile()
        {
            _fileHelper.WriteFile(_filenameHelper.GetGuessedWordsFilename(), _guessedWords);
        }

        public Dictionary GetDictionary()
        {
            return _dictionary;
        }
    }
}