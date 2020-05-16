using System.Collections.Generic;
using System.IO.Enumeration;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class AddWordToGuessedWordsTest
    {
        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordHelper _wordHelper;
        private readonly FileHelper _fileHelper;

        private const string GuessedWordsFilename = "./test-guessed-words";
        private const string DictionaryFilename = "./guessed-words-dictionary";
        
        [Fact]
        public void AllWordsGuessedAndTheirStatusesShouldBeAddedToGuessedWordsList()
        {
            var words = new List<string> {"cow", "dog", "frog", "pigeon"};
            
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = words[0],
                        PermanentDefinition = null,
                        TemporaryDefinition = null,
                        Status = WordStatus.Permanent
                    },
                    new WordData
                    {
                        Word = words[1],
                        PermanentDefinition = null,
                        TemporaryDefinition = null,
                        Status = WordStatus.Temporary
                    },
                    new WordData()
                    {
                        Word = words[2],
                        PermanentDefinition = null,
                        TemporaryDefinition = null,
                        Status = WordStatus.Suffix
                    },
                    new WordData()
                    {
                        Word = words[3],
                        PermanentDefinition = null,
                        TemporaryDefinition = null,
                        Status = WordStatus.DoesNotExist
                    }
                }
            };

            TestFileHelper.CreateCustomFile(DictionaryFilename, dictionary);

            // _fileHelper.ReadDictionary(filename).Returns(dictionary);
            
            var _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            
            // Needs to read from teh dictionary
            // Needs to write the guessed words
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, words[0]);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, words[1]);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, words[2]);
            _wordService.AddWordToGuessedWords(DictionaryFilename, GuessedWordsFilename, words[3]);

            var json = TestFileHelper.Read(GuessedWordsFilename);
            var guessedWords = JsonConvert.DeserializeObject<GuessedWords>(json);
            
            guessedWords
                .Should()
                .BeEquivalentTo(new List<WordData>
                {
                    new WordData
                    {
                        Word = words[0],
                        Status = WordStatus.Permanent
                    },
                    new WordData
                    {
                        Word = words[1],
                        Status = WordStatus.Temporary
                    },
                    new WordData()
                    {
                        Word = words[2],
                        Status = WordStatus.Suffix
                    },
                    new WordData()
                    {
                        Word = words[3],
                        Status = WordStatus.DoesNotExist
                    }
                });
        }
    }
}