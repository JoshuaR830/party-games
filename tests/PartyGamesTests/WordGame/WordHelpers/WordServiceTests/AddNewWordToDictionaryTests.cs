using System;
using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    [Collection("Files")]
    public class AddNewWordToDictionaryTests : IDisposable
    {
        private const string Filename = "./add-new-word-to-file.json";
        private WordService _wordService;

        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordHelper _wordHelper;
        private IFilenameHelper _filenameHelper;
        private readonly FileHelper _fileHelper;
        
        public AddNewWordToDictionaryTests()
        {
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordHelper = Substitute.For<IWordHelper>();
            _fileHelper = new FileHelper();
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            _filenameHelper.GetGuessedWordsFilename().Returns(Filename);

            if (File.Exists(Filename))
                File.Delete(Filename);
            
            TestFileHelper.Create(Filename);
        }

        [Fact]
        public void WhenADefinitionIsSetThenItShouldBeWrittenToTheDictionary()
        {
            var newWord = "new";
            var newDefinition = "Something that has only just come into existence";
            
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.AddNewWordToDictionary(Filename, newWord, newDefinition);
            _wordService.UpdateDictionaryFile();

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<Dictionary>(response);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = newWord,
                PermanentDefinition = newDefinition,
                TemporaryDefinition = null,
                Status = WordStatus.Permanent
            });
        }

        [Fact]
        public void WhenNoDefinitionIsEnteredTheWordShouldStillBeWritten()
        {
            var newWord = "new";
            var newDefinition = "";

            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            _wordService.AddNewWordToDictionary(Filename, newWord, newDefinition);
            _wordService.UpdateDictionaryFile();

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<Dictionary>(response);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = newWord,
                PermanentDefinition = newDefinition,
                TemporaryDefinition = null,
                Status = WordStatus.Permanent
            });
        }

        public void Dispose()
        {
            if(File.Exists(Filename))
                File.Delete(Filename);
        }
    }
}