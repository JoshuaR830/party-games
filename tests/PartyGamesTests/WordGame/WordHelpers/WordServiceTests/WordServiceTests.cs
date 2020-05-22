using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class WordServiceTests
    {
        private readonly IWordHelper _wordHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private IWordDefinitionHelper _wordDefinitionHelper;
        private IFileHelper _fileHelper;
        private IFilenameHelper _filenameHelper;

        private const string Filename = "";

        public WordServiceTests()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);

            _wordHelper = Substitute.For<IWordHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _fileHelper = Substitute.For<IFileHelper>();
        }
        
        [Fact]
        public void WhenWordExistsInTheDictionaryThenTheWordServiceShouldReturnTrue()
        {
            var word = "sheep";
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(true);
            
            var wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var response = wordService.GetWordStatus(Filename, word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenWordIsPluralThenWordServiceShouldReturnTrue()
        {
            var word = "ducks";
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(false);

            _wordHelper
                .StrippedSuffixDictionaryCheck(Filename, word)
                .Returns(true);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var response = wordService.GetWordStatus(Filename, word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotInLocalDictionaryThenWordServiceShouldReturnFalse()
        {
            var word = "sheeps";
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(false);

            _wordHelper
                .StrippedSuffixDictionaryCheck(Filename, word)
                .Returns(false);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var response = wordService.GetWordStatus(Filename, word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordIsNotAPluralButExistsInSingularThenWordServiceShouldReturnFalse()
        {
            var word = "sheeps";
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(false);
            
            _wordExistenceHelper
                .DoesWordExist("sheep")
                .Returns(true);

            _wordHelper
                .StrippedSuffixDictionaryCheck(Filename, word)
                .Returns(false);

            _wordHelper
                .CheckWordWithEndingExists(word, "sheep")
                .Returns(false);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var response = wordService.GetWordStatus(Filename, word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenAWordFormattedAsPluralDoesNotExistInTheSingularThenWordServiceShouldReturnFalse()
        {
            var word = "notawords";
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(false);
            
            _wordExistenceHelper
                .DoesWordExist(Arg.Any<string>())
                .Returns(false);

            _wordHelper
                .StrippedSuffixDictionaryCheck(Filename, word)
                .Returns(false);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var response = wordService.GetWordStatus(Filename, word);
            response.Should().BeFalse();
        }

        [Fact]
        public void WhenWordIsInTheDictionaryThenACallShouldBeMadeToGetTheDefinition()
        {
            var word = "sheep";
            
            _wordDefinitionHelper.ClearReceivedCalls();
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(true);

            _wordDefinitionHelper.GetDefinitionForWord(word).Returns("An absolutely baaing animal");

            var wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var response = wordService.GetDefinition(Filename, word);

            _wordDefinitionHelper.Received().GetDefinitionForWord(word);
            response.Should().Be("An absolutely baaing animal");
        }
        
        [Fact]
        public void WhenWordIsNotInTheDictionaryThenNoCallShouldBeMadeToGetDefinition()
        {
            var word = "sheep";
            
            _wordDefinitionHelper.ClearReceivedCalls();
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(false);
            
            _wordHelper
                .StrippedSuffixDictionaryCheck(Filename, word)
                .Returns(false);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var response = wordService.GetDefinition(Filename, word);

            _wordDefinitionHelper
                .DidNotReceive()
                .GetDefinitionForWord(word);

            response.Should().Be(null);
        }
    }
}