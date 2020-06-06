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
    public class AutomaticallyAddNewWordToDictionaryTests : IDisposable
    {
        private const string Filename = "./add-new-word-to-file.json";
        private WordService _wordService;
        private TemporaryDefinitionHelper _temporaryDefinitionHelper;

        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordHelper _wordHelper;
        private readonly IFilenameHelper _filenameHelper;
        private readonly FileHelper _fileHelper;
        private WordDictionary _wordDictionary;

        public AutomaticallyAddNewWordToDictionaryTests()
        {
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordHelper = Substitute.For<IWordHelper>();
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            _filenameHelper.GetGuessedWordsFilename().Returns(Filename);
            _fileHelper = new FileHelper(_filenameHelper);
            
            
            if (File.Exists(Filename))
                File.Delete(Filename);
            
            TestFileHelper.Create(Filename);
            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var json = TestFileHelper.Read(Filename);
            _wordDictionary = _wordService.GetDictionary();
        }

        [Fact]
        public void WhenAWordAndDefinitionAreSetTheWordShouldBeAddedToTheDictionary()
        {
            var newWord = "news";
            var temporaryDefinition = "Something that has only just come into existence";
            
            _temporaryDefinitionHelper = new TemporaryDefinitionHelper(_fileHelper, _filenameHelper);
            _temporaryDefinitionHelper.AutomaticallySetTemporaryDefinitionForWord(_wordDictionary, newWord, temporaryDefinition);
            _wordService.UpdateDictionaryFile();

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(response);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = newWord,
                PermanentDefinition = null,
                TemporaryDefinition = temporaryDefinition,
                Status = WordStatus.Suffix
            });
        }

        [Fact]
        public void WhenNoDefinitionIsEnteredTheWordShouldStillBeWritten()
        {
            var newWord = "news";
            var temporaryDefinition = "";

            _temporaryDefinitionHelper = new TemporaryDefinitionHelper(_fileHelper, _filenameHelper);
            _temporaryDefinitionHelper.AutomaticallySetTemporaryDefinitionForWord(_wordDictionary, newWord, temporaryDefinition);
            _wordService.UpdateDictionaryFile();

            var response = TestFileHelper.Read(Filename);

            var dictionary = JsonConvert.DeserializeObject<WordDictionary>(response);

            dictionary.Words.Should().ContainEquivalentOf(new WordData
            {
                Word = newWord,
                PermanentDefinition = null,
                TemporaryDefinition = temporaryDefinition,
                Status = WordStatus.Suffix
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