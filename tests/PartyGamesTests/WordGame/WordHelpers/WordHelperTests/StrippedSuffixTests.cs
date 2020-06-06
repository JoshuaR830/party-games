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
        private readonly ITemporaryDefinitionHelper _temporaryDefinitionHelper;

        private readonly string Filename = "";

        public StrippedSuffixTests()
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
        public void WhenWordHasOneLetterEnding()
        {
            var word = "cheeses";
            
            _webDictionaryRequestHelper
                .MakeContentRequest("cheese")
                .Returns("Word forms: there are some cheeses over there");

            var wordHelperUnderTest = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);
            var response = wordHelperUnderTest.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeTrue();
        }

        [Fact]
        public void WhenNotAWordButEndingInOneLetter()
        {
            var word = "REALLYNOTAWORDS";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasTwoLetterEnding()
        {
            var word = "BOXES";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("box")
                .Returns("Word forms: there are some boxes over there");

            var response = wordHelper.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordButEndingInTwoLetters()
        {
            var word = "REALLYNOTAWORDES";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasThreeLetterEnding()
        {
            var word = "KICKING";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("kick")
                .Returns("Word forms: This is really kicking it");

            var response = wordHelper.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordButEndingInThreeLetters()
        {
            var word = "reallynotawording";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeFalse();
        }
        
        [Fact]
        public void WhenWordHasFourLetterEnding()
        {
            var word = "RUNNING";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);
            
            _webDictionaryRequestHelper
                .MakeContentRequest("run")
                .Returns("Word forms: got to love running in the park");

            var response = wordHelper.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeTrue();
        }

        [Fact]
        public void WhenNotAWordButEndingInfourLetters()
        {
            var word = "reallynotawordning";
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _temporaryDefinitionHelper);

            _webDictionaryRequestHelper
                .MakeContentRequest("reallynotaword")
                .Returns("This really isn't a word");

            var response = wordHelper.StrippedSuffixDictionaryCheck(new WordDictionary(), word);
            response.Should().BeFalse();
        }
    }
}