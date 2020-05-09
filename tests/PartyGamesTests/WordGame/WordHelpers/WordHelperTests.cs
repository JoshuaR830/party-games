using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers
{
    public class WordHelperTests
    {
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;

        public WordHelperTests()
        {
            _webDictionaryRequestHelper = Substitute.For<IWebDictionaryRequestHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();

            _wordExistenceHelper
                .DoesWordExist(Arg.Any<string>())
                .Returns(true);
        }

        [Fact]
        public void WhenWordExistsResponseShouldBeTrue()
        {
            var word = "sheep";
            var responseString = "There once was a sheep called Ollie, who jumped through the hedge by a lorry, the man hit the brakes, the sheep made mistakes, but all ended well and is jolly";
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            var response = wordHelper.CheckWordEndingExists(word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenWordDoesNotExistResponseShouldBeFalse()
        {
            var nonExistentWord = "sheeps";

            var responseString = "There once was a sheep called simon, whose fleece was as shiny as diamond, he ate grass all day, through the night he did play, and now he just sleeps through the day";

            _webDictionaryRequestHelper
                .MakeContentRequest(nonExistentWord)
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            var response = wordHelper.CheckWordEndingExists(nonExistentWord);
            response.Should().BeFalse();
        }

        [Fact]
        public void WhenWordHasOneLetterEnding()
        {
            var word = "cheeses";
            var ending = "s";
            
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("There are some cheeses over there");

            var wordHelperUnderTest = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            var response = wordHelperUnderTest.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordButEndingInOneLetter()
        {
            var word = "reallynotawords";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasTwoLetterEnding()
        {
            var word = "boxes";
            var ending = "es";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("There are some boxes over there");

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordButEndingInTwoLetters()
        {
            var word = "reallynotawordes";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasThreeLetterEnding()
        {
            var word = "kicking";
            var ending = "ing";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("This is really kicking it");

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordButEndingInThreeLetters()
        {
            var word = "reallynotawording";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasFourLetterEnding()
        {
            var word = "running";
            var ending = "ning";

            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("Got to love running in the park");

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();
        }

        [Fact]
        public void WhenNotAWordButEndingInfourLetters()
        {
            var word = "reallynotawordning";

            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper);

            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeFalse();
        }
    }
}