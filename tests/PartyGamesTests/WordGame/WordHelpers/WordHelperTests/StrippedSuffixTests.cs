using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordHelperTests
{
    public class StrippedSuffixTests
    {
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IFileHelper _fileHelper;
        private readonly IWordService _wordService;

        private readonly string Filename = "";

        public StrippedSuffixTests()
        {
            _webDictionaryRequestHelper = Substitute.For<IWebDictionaryRequestHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _fileHelper = Substitute.For<IFileHelper>();
            _wordService = Substitute.For<IWordService>();

            _wordExistenceHelper
                .DoesWordExist(Arg.Any<string>())
                .Returns(true);
        }
        
        [Fact]
        public void WhenWordHasOneLetterEnding()
        {
            var word = "cheeses";
            
            _webDictionaryRequestHelper
                .MakeContentRequest("cheese")
                .Returns("Word forms: there are some cheeses over there");

            var wordHelperUnderTest = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            var response = wordHelperUnderTest.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeTrue();
        }

        [Fact]
        public void WhenNotAWordButEndingInOneLetter()
        {
            var word = "reallynotawords";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasTwoLetterEnding()
        {
            var word = "boxes";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("box")
                .Returns("Word forms: there are some boxes over there");

            var response = wordHelper.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordButEndingInTwoLetters()
        {
            var word = "reallynotawordes";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasThreeLetterEnding()
        {
            var word = "kicking";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("kick")
                .Returns("Word forms: This is really kicking it");

            var response = wordHelper.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordButEndingInThreeLetters()
        {
            var word = "reallynotawording";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasFourLetterEnding()
        {
            var word = "running";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("run")
                .Returns("Word forms: got to love running in the park");

            var response = wordHelper.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeTrue();
        }

        [Fact]
        public void WhenNotAWordButEndingInfourLetters()
        {
            var word = "reallynotawordning";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);

            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(Filename, word);
            response.Should().BeFalse();
        }
    }
}