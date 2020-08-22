using System;
using System.IO;
using Amazon.Lambda;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
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
        
        private readonly IWordHelper _wordHelper;
        private IFilenameHelper _filenameHelper;
        private readonly FileHelper _fileHelper;
        private IAmazonLambda _lambda;

        public AddNewWordToDictionaryTests()
        {
            _wordHelper = Substitute.For<IWordHelper>();
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            _filenameHelper.GetGuessedWordsFilename().Returns(Filename);
            _fileHelper = new FileHelper(_filenameHelper);
            _lambda = Substitute.For<IAmazonLambda>();

            if (File.Exists(Filename))
                File.Delete(Filename);
            
            TestFileHelper.Create(Filename);
        }

        [Fact]
        public void WhenADefinitionIsSetThenItShouldBeWrittenToTheDictionary()
        {
            var newWord = "new";
            var newDefinition = "Something that has only just come into existence";
            
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.AddNewWordToDictionary(Filename, newWord, newDefinition);
            _wordService.UpdateDictionaryFile();

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(response);

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

            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordService.AddNewWordToDictionary(Filename, newWord, newDefinition);
            _wordService.UpdateDictionaryFile();

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(response);

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
            
            WordDictionaryGetter.WordDictionary.Remove(Filename);
        }
    }
}