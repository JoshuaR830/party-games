using System;
using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class WordStatusTests : IDisposable
    {
        private WordService _wordService;
        private IWordDefinitionHelper _wordDefinitionHelper;
        private IFilenameHelper _filenameHelper;
        private IWordExistenceHelper _wordExistenceHelper;
        private IWordHelper _wordHelper;
        private ITemporaryDefinitionHelper _temporaryDefinitionHelper;
        private FileHelper _fileHelper;
        private const string Filename = "./word-status-tests.json";
        
        public WordStatusTests()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);

            TestFileHelper.Create(Filename);
            
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordHelper = Substitute.For<IWordHelper>();
            _fileHelper = new FileHelper();
        }
        
        [Fact]
        public void WhenDefinitionAutomaticallySourcedFromSuffixStatusShouldBeSuffix()
        {
            var word = "pelicans";
            var temporaryDefinition = TestFileHelper.PelicanTemporaryDefinition;
            _temporaryDefinitionHelper = new TemporaryDefinitionHelper(_fileHelper);
            _temporaryDefinitionHelper.AutomaticallySetTemporaryDefinitionForWord(Filename, word, temporaryDefinition);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.UpdateExistingWord(Filename, word, newDefinition);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.AddNewWordToDictionary(Filename, word, newDefinition);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, true);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.ToggleIsWordInDictionary(Filename, word, false);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

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
        }
    }
}