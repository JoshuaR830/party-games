using System;
using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordHelperTests
{
    public class AutomaticallySetTemporaryDefinitions : IDisposable
    {
        private readonly IWebDictionaryRequestHelper _webDictionaryRequestHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IWordHelper _wordHelper;
        private readonly FileHelper _fileHelper;
        private readonly WordService _wordService;
        private const string Filename = "./automatically-set-temporary-definitions";
        
        public AutomaticallySetTemporaryDefinitions()
        {
            TestFileHelper.Create(Filename);
            _webDictionaryRequestHelper = Substitute.For<IWebDictionaryRequestHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _fileHelper = new FileHelper();
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper);
        }

        [Fact]
        public void WhenPluralIsAddedThenTemporaryDefinitionIsSetToSingular()
        {
            var word = "sloths";
            var shortenedWord = "sloth";
            
            _wordDefinitionHelper.GetDefinitionForWord(shortenedWord).Returns(TestFileHelper.SlothTemporaryDefinition);
            _webDictionaryRequestHelper.MakeContentRequest(shortenedWord).Returns("sloth word forms plural sloths");
            _wordExistenceHelper.DoesWordExist(shortenedWord).Returns(true);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            wordHelper.StrippedSuffixDictionaryCheck(Filename, word);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = "sloths",
                PermanentDefinition = null,
                TemporaryDefinition = TestFileHelper.SlothTemporaryDefinition
            });
        }
        
        [Fact]
        public void WhenDoingIsAddedThenTemporaryDefinitionIsSetToBaseWord()
        {
            var word = "slothning";
            var shortenedWord = "sloth";

            _wordDefinitionHelper.GetDefinitionForWord("sloth").Returns(TestFileHelper.SlothTemporaryDefinition);
            _webDictionaryRequestHelper.MakeContentRequest(shortenedWord).Returns("sloth word forms doing slothning");
            _wordExistenceHelper.DoesWordExist(shortenedWord).Returns(true);
            
            var wordHelper = new WordHelper(_webDictionaryRequestHelper, _wordExistenceHelper, _wordDefinitionHelper, _fileHelper, _wordService);
            wordHelper.StrippedSuffixDictionaryCheck(Filename, word);

            var json = TestFileHelper.Read(Filename);
            var dictionary = JsonConvert.DeserializeObject<Dictionary>(json);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = "slothning",
                PermanentDefinition = null,
                TemporaryDefinition = TestFileHelper.SlothTemporaryDefinition
            });
        }

        public void Dispose()
        {
            if(File.Exists(Filename))
                File.Delete(Filename);
        }
    }
}