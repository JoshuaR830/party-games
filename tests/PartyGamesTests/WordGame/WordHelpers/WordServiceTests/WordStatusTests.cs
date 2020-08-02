using System;
using System.IO;
using Amazon.Lambda;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using NSubstitute.Exceptions;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class WordStatusTests : IDisposable
    {
        private WordService _wordService;
        private IFilenameHelper _filenameHelper;
        private ITemporaryDefinitionHelper _temporaryDefinitionHelper;
        private FileHelper _fileHelper;
        private WordDictionary _wordDictionary;
        private IAmazonLambda _lambda;
        private const string Filename = "./word-status-tests.json";
        
        public WordStatusTests()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper
                .GetGuessedWordsFilename()
                .Returns(Filename);
            
            _filenameHelper
                .GetDictionaryFilename()
                .Returns(Filename);

            TestFileHelper.Create(Filename);
            
            _fileHelper = new FileHelper(_filenameHelper);
            _lambda = Substitute.For<IAmazonLambda>();
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordDictionary = _wordService.GetDictionary();
        }
        
        [Fact]
        public void WhenDefinitionAutomaticallySourcedFromSuffixStatusShouldBeSuffix()
        {
            var word = "pelicans";
            var temporaryDefinition = TestFileHelper.PelicanTemporaryDefinition;
            _temporaryDefinitionHelper = new TemporaryDefinitionHelper(_fileHelper, _filenameHelper);
            _temporaryDefinitionHelper.AutomaticallySetTemporaryDefinitionForWord(_wordDictionary, word, temporaryDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = temporaryDefinition,
                PermanentDefinition = null,
                Status = WordStatus.Suffix
            });
        }
        
        [Fact]
        public void WhenUserUpdatesExistingDefinition()
        {
            var word = "pelican";
            var newDefinition = TestFileHelper.PelicanPermanentDefinition;
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.UpdateExistingWord(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = TestFileHelper.PelicanTemporaryDefinition,
                PermanentDefinition = newDefinition,
                Status = WordStatus.Permanent
            });
        }
        
        [Fact]
        public void WhenUserAddsNewWordToDictionary()
        {
            var word = "hello";
            var newDefinition = "A friendly greeting";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.AddNewWordToDictionary(Filename, word, newDefinition);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = null,
                PermanentDefinition = newDefinition,
                Status = WordStatus.Permanent
            });
        }
        
        [Fact]
        public void WhenUserMarksPermanentWordAsNotExistingTheStatusShouldBeSetToDoesNotExist()
        {
            var word = "sheep";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = TestFileHelper.SheepTemporaryDefinition,
                PermanentDefinition = TestFileHelper.SheepPermanentDefinition,
                Status = WordStatus.DoesNotExist
            });
        }
        
        [Fact]
        public void WhenUserMarksTemporaryWordAsNotExistingTheStatusShouldBeSetToDoesNotExist()
        {
            var word = "lion";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = TestFileHelper.LionTemporaryDefinition,
                PermanentDefinition = TestFileHelper.LionPermanentDefinition,
                Status = WordStatus.DoesNotExist
            });
        }
        
        [Fact]
        public void WhenUserMarksSuffixStatusWordAsNotExistingTheStatusShouldBeSetToDoesNotExist()
        {
            var word = "boxing";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = TestFileHelper.BoxingTemporaryDefinition,
                PermanentDefinition = null,
                Status = WordStatus.DoesNotExist
            });
        }
        
        [Fact]
        public void WhenUserMarksNonExistingWordWithPermanentDefinitionAsExistingThenStatusShouldBePermanent()
        {
            var word = "dodo";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = null,
                PermanentDefinition = TestFileHelper.DodoPermanentDefinition,
                Status = WordStatus.Permanent
            });
        }
        
        [Fact]
        public void WhenUserMarksNonExistingWordWithATemporaryDefinitionButNoPermanentDefinitionAsExistingThenStatusShouldBeTemporary()
        {
            var word = "unicorn";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = TestFileHelper.UnicornTemporaryDefinition,
                PermanentDefinition = null,
                Status = WordStatus.Temporary
            });
        }
        
        [Fact]
        public void WhenUserMarksNonExistingWordWithNoDefinitionsAsExistingThenStatusShouldBeTemporary()
        {
            var word = "dinosaur";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = null,
                PermanentDefinition = null,
                Status = WordStatus.Temporary
            });
        }
        
        [Fact]
        public void WhenUserMarksPermanentWordWithNoDefinitionsAsExistingThenStatusShouldBePermanent()
        {
            var word = "sheep";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = TestFileHelper.SheepTemporaryDefinition,
                PermanentDefinition = TestFileHelper.SheepPermanentDefinition,
                Status = WordStatus.Permanent
            });
        }
        
        [Fact]
        public void WhenUserMarksNonExistingWordAsNonExistingThenStatusShouldBeNonExisting()
        {
            var word = "dinosaur";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);
            _wordService.UpdateDictionaryFile();

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = null,
                PermanentDefinition = null,
                Status = WordStatus.DoesNotExist
            });
        }

        public void Dispose()
        {
            if(File.Exists(Filename))
                File.Delete(Filename);
            
            WordDictionaryGetter.WordDictionary.Remove(Filename);
        }
    }
}