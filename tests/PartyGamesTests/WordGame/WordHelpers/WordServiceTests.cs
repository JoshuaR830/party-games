using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers
{
    public class WordServiceTests
    {
        private readonly IWordHelper _wordHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;

        public WordServiceTests()
        {
            _wordHelper = Substitute.For<IWordHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
        }
        
        [Fact]
        public void WhenWordExistsInTheDictionaryThenTheWordServiceShouldReturnTrue()
        {
            var word = "sheep";
            
            _wordExistenceHelper
                .DoesWordExist(word)
                .Returns(true);
            
            var wordService = new WordService(_wordExistenceHelper, _wordHelper);
            var response = wordService.GetWordStatus(word);
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
                .StrippedSuffixDictionaryCheck(word)
                .Returns(true);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper);
            var response = wordService.GetWordStatus(word);
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
                .StrippedSuffixDictionaryCheck(word)
                .Returns(false);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper);
            var response = wordService.GetWordStatus(word);
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
                .StrippedSuffixDictionaryCheck(word)
                .Returns(false);

            _wordHelper
                .CheckWordEndingExists(word)
                .Returns(false);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper);
            var response = wordService.GetWordStatus(word);
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
                .StrippedSuffixDictionaryCheck(word)
                .Returns(false);

            var wordService = new WordService(_wordExistenceHelper, _wordHelper);
            var response = wordService.GetWordStatus(word);
            response.Should().BeFalse();
        }
    }
}