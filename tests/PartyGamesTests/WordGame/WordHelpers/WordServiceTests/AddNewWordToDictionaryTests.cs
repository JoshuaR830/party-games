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
    public class AddNewWordToDictionaryTests : IDisposable
    {
        private const string Filename = "/add-new-word-to-file.json";
        private WordService _wordService;

        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordHelper _wordHelper;
        
        public AddNewWordToDictionaryTests()
        {
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordHelper = Substitute.For<IWordHelper>();
            
            if (File.Exists(Filename))
                File.Delete(Filename);
            
            TestFileHelper.Create(Filename);
        }

        [Fact]
        public void Something()
        {
            var newWord = "new";
            var newDefinition = "Something that has only just come into existence";
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper);

            _wordService.AddNewWordToDictionary(newWord, newDefinition);

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<Dictionary>(response);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = newWord,
                PermanentDefinition = newDefinition,
                TemporaryDefinition = null
            });
        }

        [Fact]
        public void WhenNoDefinitionIsEnteredTheWordShouldStillBeWritten()
        {
            var newWord = "new";
            var newDefinition = "";

            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper);

            _wordService.AddNewWordToDictionary(newWord, newDefinition);

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<Dictionary>(response);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = newWord,
                PermanentDefinition = newDefinition,
                TemporaryDefinition = null
            });
        }

        public void Dispose()
        {
            if(File.Exists(Filename))
                File.Delete(Filename);
        }
    }
}