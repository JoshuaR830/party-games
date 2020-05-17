using System;
using System.IO.Enumeration;
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

        public WordService(IWordExistenceHelper wordExistenceHelper, IWordHelper wordHelper, IWordDefinitionHelper wordDefinitionHelper, IFileHelper fileHelper)
        {
            _wordExistenceHelper = wordExistenceHelper;
            _wordHelper = wordHelper;
            _wordDefinitionHelper = wordDefinitionHelper;
            _fileHelper = fileHelper;
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

        public void AddNewWordToDictionary(string filename, string word, string definition)
        {
            var dictionary = _fileHelper.ReadDictionary(filename);
            
            dictionary.Words.Add(new WordData
            {
                Word = word,
                PermanentDefinition = definition,
                TemporaryDefinition = null,
                Status = WordStatus.Permanent
            });
            
            _fileHelper.WriteDictionary(filename, dictionary);
        }

        public void UpdateExistingWord(string filename, string word, string definition)
        {
            if (word == "" || definition == "")
                return;

            var dictionary = _fileHelper.ReadDictionary(filename);

            var wordList = dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!wordList.Any())
                return;

            var item = dictionary.Words.IndexOf(wordList.First());
            dictionary.Words[item].PermanentDefinition = definition;
            dictionary.Words[item].Status = WordStatus.Permanent;

            _fileHelper.WriteDictionary(filename, dictionary);
        }

        public void ToggleIsWordInDictionary(string filename, string word)
        {
            var dictionary = _fileHelper.ReadDictionary(filename);

            var items = dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!items.Any())
                return;

            var item = items.First();

            var index = dictionary.Words.IndexOf(item);

            if (item.Status != WordStatus.DoesNotExist)
            {
                dictionary.Words[index].Status = WordStatus.DoesNotExist;
                _fileHelper.WriteDictionary(filename, dictionary);
                return;
            }
            
            dictionary.Words[index].Status = item.PermanentDefinition != null ? WordStatus.Permanent : WordStatus.Temporary;
            
            _fileHelper.WriteDictionary(filename, dictionary);
        }

        public void AddWordToGuessedWords(string dictionaryFilename, string guessedWordsFilename, string word)
        {
            var dictionary = _fileHelper.ReadDictionary(dictionaryFilename);
            var items = dictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

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