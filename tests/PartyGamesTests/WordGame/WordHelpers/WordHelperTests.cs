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

        public WordHelperTests()
        {
            _webDictionaryRequestHelper = Substitute.For<IWebDictionaryRequestHelper>();
        }

        [Fact]
        public void WhenWordExistsResponseShouldBeTrue()
        {
            var word = "sheep";
            var responseString = "There once was a sheep called Ollie, who jumped through the hedge by a lorry, the man hit the brakes, the sheep made mistakes, but all ended well and is jolly";
            
            _webDictionaryRequestHelper
                .MakeContentRequest(word)
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper);
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
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper);
            var response = wordHelper.CheckWordEndingExists(nonExistentWord);
            response.Should().BeFalse();
        }

        [Fact]
        public void WhenWordHasOneLetterEnding()
        {
            var word = "cheeses";
            var ending = "s";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper);
            wordHelper
                .CheckWordEndingExists(word)
                .Returns(true);

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenWordHasTwoLetterEnding()
        {
            var word = "fishes";
            var ending = "es";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper);
            wordHelper
                .CheckWordEndingExists(word)
                .Returns(true);

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();

        }
        
        [Fact]
        public void WhenWordHasThreeLetterEnding()
        {
            var word = "kicking";
            var ending = "ing";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper);
            wordHelper
                .CheckWordEndingExists(word)
                .Returns(true);

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();

        }
        
        [Fact]
        public void WhenWordHasFourLetterEnding()
        {
            var word = "running";
            var ending = "ning";

            var wordHelper = new WordHelper(_webDictionaryRequestHelper);
            wordHelper
                .CheckWordEndingExists(word)
                .Returns(true);

            var response = wordHelper.StrippedSuffixDictionaryCheck(word);
            response.Should().BeTrue();
        }
    }
}