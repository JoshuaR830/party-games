using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class WordStatusTests
    {
        private WordService _wordService;
        private IWordDefinitionHelper _wordDefinitionHelper;
        private IWordExistenceHelper _wordExistenceHelper;
        private IWordHelper _wordHelper;
        private FileHelper _fileHelper;
        private const string filename = "./word-status-tests.json";
        
        public WordStatusTests()
        {
            TestFileHelper.Create(filename);
            
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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.AutomaticallySetTemporaryDefinitionForWord(filename, word, temporaryDefinition);

            var json = TestFileHelper.Read(filename);
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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.UpdateExistingWord(filename, word, newDefinition);

            var json = TestFileHelper.Read(filename);
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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.AddNewWordToDictionary(filename, word, newDefinition);

            var json = TestFileHelper.Read(filename);
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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.ToggleIsWordInDictionary(filename, word);

            var json = TestFileHelper.Read(filename);
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
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.ToggleIsWordInDictionary(filename, word);

            var json = TestFileHelper.Read(filename);
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
        public void WhenUserMarksSuffixStatusWordAsNotExistingTheStatusShouldBeSetToDoesNotExist()
        {
            var word = "boxing";
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.ToggleIsWordInDictionary(filename, word);

            var json = TestFileHelper.Read(filename);
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
        public void WhenUserMarksNonExistingWordWithPermanentDefinitionAsExistingThenStatusShouldBePermanent()
        {
            var word = "dodo";
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.ToggleIsWordInDictionary(filename, word);

            var json = TestFileHelper.Read(filename);
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
        public void WhenUserMarksNonExistingWordWithATemporaryDefinitionButNoPermanentDefinitionAsExistingThenStatusShouldBeTemporary()
        {
            var word = "unicorn";
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.ToggleIsWordInDictionary(filename, word);

            var json = TestFileHelper.Read(filename);
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
        public void WhenUserMarksNonExistingWordWithNoDefinitionsAsExistingThenStatusShouldBeTemporary()
        {
            var word = "dinosaur";
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
            _wordService.ToggleIsWordInDictionary(filename, word);

            var json = TestFileHelper.Read(filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = word,
                TemporaryDefinition = TestFileHelper.SheepTemporaryDefinition,
                PermanentDefinition = TestFileHelper.SheepPermanentDefinition,
                Status = WordStatus.Temporary
            });
        }
    }
}