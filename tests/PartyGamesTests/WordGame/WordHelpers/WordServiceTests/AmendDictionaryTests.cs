using System;
using System.Collections.Generic;
using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class AmendDictionaryTests : IDisposable
    {
        private const string Filename = "./amend-dictionary-tests.json";
        private IWordExistenceHelper _wordExistenceHelper;
        private IWordHelper _wordHelper;
        private IWordDefinitionHelper _wordDefinitionHelper;
        private FileHelper _fileHelper;
        private IFilenameHelper _filenameHelper;

        private readonly WordService _wordService;

        public AmendDictionaryTests()
        {
            
            var data = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "pelican",
                        Status = WordStatus.Temporary,
                        PermanentDefinition = null,
                        TemporaryDefinition = null
                    }
                }
            };
            
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);

            TestFileHelper.CreateCustomFile(Filename, data);
            _fileHelper = new FileHelper();
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
        }

        [Fact]
        public void WhenDictionaryContainsWordThenTheDetailsShouldBeUpdated()
        {
            var word = "pelican";
            var definition = TestFileHelper.PelicanPermanentDefinition;
            _wordService.AmendDictionary(Filename, word, definition);

            var json = TestFileHelper.Read(Filename);
            var response = JsonConvert.DeserializeObject<Dictionary>(json);
            
            response.Should().BeEquivalentTo(new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "pelican",
                        Status = WordStatus.Permanent,
                        PermanentDefinition = definition,
                        TemporaryDefinition = null
                    }
                }
            });
        }

        [Fact]
        public void WhenDictionaryDoesNotContainWordThenTheNewWordShouldBeAdded()
        {
            var word = "sheep";
            var definition = TestFileHelper.SheepPermanentDefinition; 
            _wordService.AmendDictionary(Filename, word, definition);
            
            var json = TestFileHelper.Read(Filename);
            var response = JsonConvert.DeserializeObject<Dictionary>(json);
            
            response.Should().BeEquivalentTo(new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "pelican",
                        Status = WordStatus.Temporary,
                        PermanentDefinition = null,
                        TemporaryDefinition = null
                    },
                    new WordData
                    {
                        Word = "sheep",
                        Status = WordStatus.Permanent,
                        PermanentDefinition = definition,
                        TemporaryDefinition = null
                    }
                }
            });
        }

        public void Dispose()
        {
            if (File.Exists(Filename))
                File.Delete(Filename);
        }
    }
}