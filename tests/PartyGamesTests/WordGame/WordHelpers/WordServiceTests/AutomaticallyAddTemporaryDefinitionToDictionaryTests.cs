﻿using System;
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
    public class AutomaticallyAddNewWordToDictionaryTests : IDisposable
    {
        private const string Filename = "./add-new-word-to-file.json";
        private WordService _wordService;
        private TemporaryDefinitionHelper _temporaryDefinitionHelper;
        
        private readonly IFilenameHelper _filenameHelper;
        private readonly FileHelper _fileHelper;
        private WordDictionary _wordDictionary;
        private IAmazonLambda _lambda;

        public AutomaticallyAddNewWordToDictionaryTests()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            _filenameHelper.GetGuessedWordsFilename().Returns(Filename);
            _fileHelper = new FileHelper(_filenameHelper);
            _lambda = Substitute.For<IAmazonLambda>();


            if (File.Exists(Filename))
                File.Delete(Filename);
            
            TestFileHelper.Create(Filename);
            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
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