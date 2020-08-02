using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Chat.WordGame.LocalDictionaryHelpers;
using Newtonsoft.Json;

namespace Chat.WordGame.WordHelpers
{
    public class WordService : IWordService
    {
        private readonly IFileHelper _fileHelper;
        private readonly IFilenameHelper _filenameHelper;
        private readonly IAmazonLambda _amazonLambda;

        private readonly GuessedWords _guessedWords;

        public WordService(IFileHelper fileHelper, IFilenameHelper filenameHelper, IAmazonLambda amazonLambda)
        {
            _fileHelper = fileHelper;
            _filenameHelper = filenameHelper;
            _amazonLambda = amazonLambda;

            if (_guessedWords == null)
            {
                Console.WriteLine("Guessed words");
                var json = _fileHelper.ReadFile(_filenameHelper.GetGuessedWordsFilename());
                _guessedWords = JsonConvert.DeserializeObject<GuessedWords>(json) ?? new GuessedWords();

            }
        }

        public async Task<bool> GetWordStatus(string word)
        {
            var wordResponse = await InvokeWordExistenceLambda(word);
            return wordResponse.IsSuccessful;
        }

        public async Task<string> GetDefinition(string word)
        {
            var wordResponse = await InvokeWordExistenceLambda(word);
            return wordResponse.WordResponse?.Definition;
        }
        
        public async Task<WordCategory> GetCategory(string word)
        {
            if (await GetWordStatus(word))
            {
                var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
                return wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList()[0].Category;
            }

            return WordCategory.None;
        }

        public void AmendDictionary(string filename, string word, string definition, WordCategory category = WordCategory.None)
        {
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());

            if (wordDictionary.Words.Any(x => x.Word.ToLower() == word.ToLower()))
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
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());

            wordDictionary.Words.Add(new WordData
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
         
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());

            var wordList = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!wordList.Any())
                return;

            var item = wordDictionary.Words.IndexOf(wordList.First());
            wordDictionary.Words[item].PermanentDefinition = definition;
            wordDictionary.Words[item].Status = WordStatus.Permanent;
            wordDictionary.Words[item].Category = category;
        }

        public void UpdateCategory(string filename, string word, WordCategory category)
        {
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());

            var wordList = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!wordList.Any())
                return;

            var item = wordDictionary.Words.IndexOf(wordList.First());
            wordDictionary.Words[item].Category = category;
        }

        public void ToggleIsWordInDictionary(string filename, string word, bool expectedNewStatus)
        {
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());

            var items = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();

            if (!items.Any())
                return;

            var item = items.First();

            var index = wordDictionary.Words.IndexOf(item);

            if (expectedNewStatus == false)
            {
                wordDictionary.Words[index].Status = WordStatus.DoesNotExist;
                return;
            }
            
            Console.WriteLine(JsonConvert.SerializeObject(wordDictionary.Words[index]));
            wordDictionary.Words[index].Status = item.PermanentDefinition != null ? WordStatus.Permanent : WordStatus.Temporary;
            Console.WriteLine(JsonConvert.SerializeObject(wordDictionary.Words[index]));
        }

        public void AddWordToGuessedWords(string dictionaryFilename, string guessedWordsFilename, string word)
        {
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());

            var items = wordDictionary.Words.Where(x => x.Word.ToLower() == word.ToLower()).ToList();
            
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
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
            _fileHelper.WriteFile(_filenameHelper.GetDictionaryFilename(), wordDictionary);
        }

        public void UpdateGuessedWordsFile()
        {
            _fileHelper.WriteFile(_filenameHelper.GetGuessedWordsFilename(), _guessedWords);
        }

        public WordDictionary GetDictionary()
        {
            var wordDictionary = _fileHelper.ReadDictionary(_filenameHelper.GetDictionaryFilename());
            return wordDictionary;
        }

        private async Task<WordResponseWrapper> InvokeWordExistenceLambda(string word)
        {
            var request = new InvokeRequest
            {
                FunctionName = "WordServiceExistenceProcessor",
                InvocationType = InvocationType.RequestResponse,
                Payload = JsonConvert.SerializeObject(word.ToLower())
            };
            
            var response = await _amazonLambda.InvokeAsync(request);

            var json = await new StreamReader(response.Payload).ReadToEndAsync();
            
            return JsonConvert.DeserializeObject<WordResponseWrapper>(json);
        }
    }
}