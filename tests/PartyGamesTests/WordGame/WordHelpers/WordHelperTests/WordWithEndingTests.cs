using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordHelperTests
{
    public class WordWithEndingTests
    {
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IFileHelper _fileHelper;
        private readonly ITemporaryDefinitionHelper _temporaryDefinitionHelper;

        public WordWithEndingTests()
        {
            _webDictionaryRequestHelper = Substitute.For<IWebDictionaryRequestHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _fileHelper = Substitute.For<IFileHelper>();
            _temporaryDefinitionHelper = Substitute.For<ITemporaryDefinitionHelper>();

            _wordExistenceHelper
                .DoesWordExist(Arg.Any<string>())
                .Returns(true);
        }
        
        [Fact]
        public void WhenWordExistsResponseShouldBeTrue()
        {
            var word = "ended";
            var responseString = "Word forms: There once was a sheep called Ollie, who jumped through the hedge by a lorry, the man hit the brakes, the sheep made mistakes, but all ended well and he's jolly";
            
            _webDictionaryRequestHelper
                .MakeContentRequest("end")
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _temporaryDefinitionHelper);
            var response = wordHelper.CheckWordWithEndingExists(word, "end");
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenWordHasWordFormsButIsNotARealWordResponseShouldBeFalse()
        {
            var nonExistentWord = "sheeps";
            var responseString = "word forms: There once was a sheep called simon, whose fleece was as shiny as diamond, he ate grass all day, word forms through the night he did play, and now he just sleeps through the day";

            _webDictionaryRequestHelper
                .MakeContentRequest("sheeps")
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _temporaryDefinitionHelper);
            var response = wordHelper.CheckWordWithEndingExists(nonExistentWord, "sheeps");
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordIsRealButTheSiteDoesNotContainWordFormsThenResponseShouldBeFalse()
        {
            var word = "diamond";
            var responseString = "There once was a sheep called simon, whose fleece was as shiny as diamond, he ate grass all day, through the night he did play, and now he just sleeps through the day";

            _webDictionaryRequestHelper
                .MakeContentRequest("diamond")
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _temporaryDefinitionHelper);
            var response = wordHelper.CheckWordWithEndingExists(word, "diamond");
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordDoesNotExistAndDoesNotReturnWordFormsThenResponseShouldBeFalse()
        {
            var nonExistentWord = "sheeps";
            var responseString = "There once was a sheep called simon, whose fleece was as shiny as diamond, he ate grass all day, through the night he did play, and now he just sleeps through the day";

            _webDictionaryRequestHelper
                .MakeContentRequest("sheeps")
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _temporaryDefinitionHelper);
            var response = wordHelper.CheckWordWithEndingExists(nonExistentWord, "sheeps");
            response.Should().BeFalse();
        }

        [Fact]
        public void WhenSiteContainsWordFormsAndWordExistsThenResponseShouldBeTrue()
        {
            var word = "diamond";
            var responseString = "Word forms: There once was a sheep called simon, whose fleece was as shiny as diamonds, he ate grass all day, through the night he did play, and now he just sleeps through the day";

            _webDictionaryRequestHelper
                .MakeContentRequest("diamond")
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _temporaryDefinitionHelper);
            var response = wordHelper.CheckWordWithEndingExists(word, "diamond");
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenWordDoesNotExistResponseShouldBeFalse()
        {
            var nonExistentWord = "sheeps";
            var responseString = "There once was a sheep called simon, whose fleece was as shiny as diamond, he ate grass all day, through the night he did play, and now he just sleeps through the day";

            _webDictionaryRequestHelper
                .MakeContentRequest("sheeps")
                .Returns(responseString);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _temporaryDefinitionHelper);
            var response = wordHelper.CheckWordWithEndingExists(nonExistentWord, "sheeps");
            response.Should().BeFalse();
        }

    }
}