using System;
using System.Collections.Generic;
using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class GetCategoryTests : IDisposable
    {
        private const string Filename = "./add-new-category-to-file.json";
        private WordService _wordService;
        private TemporaryDefinitionHelper _temporaryDefinitionHelper;

        private readonly IWordDefinitionHelper _wordDefinitionHelper;
        private readonly IWordExistenceHelper _wordExistenceHelper;
        private readonly IWordHelper _wordHelper;
        private readonly IFilenameHelper _filenameHelper;
        private readonly FileHelper _fileHelper;
        private WordDictionary _wordDictionary;
        private WordDictionary _categoriesWordDictionary;

        public GetCategoryTests()
        {
            _wordDefinitionHelper = Substitute.For<IWordDefinitionHelper>();
            _wordExistenceHelper = Substitute.For<IWordExistenceHelper>();
            _wordHelper = Substitute.For<IWordHelper>();
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            _filenameHelper.GetGuessedWordsFilename().Returns(Filename);
            _fileHelper = new FileHelper(_filenameHelper);
            
            _wordExistenceHelper.DoesWordExist(Arg.Any<string>()).Returns(true);
            
            if (File.Exists(Filename))
                File.Delete(Filename);
            
            _categoriesWordDictionary = new WordDictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Elephant",
                        Category = WordCategory.Animal
                    },
                    new WordData
                    {
                        Word = "Tree",
                        Category = WordCategory.Plant
                    },
                    new WordData
                    {
                        Word = "Bus",
                        Category = WordCategory.Vehicle
                    },
                    new WordData
                    {
                        Word = "Because",
                        Category = WordCategory.None
                    }
                }
            };
            
            
            TestFileHelper.CreateCustomFile(Filename, _categoriesWordDictionary);

            _wordService = new WordService(_wordExistenceHelper, _wordHelper, _wordDefinitionHelper, _fileHelper, _filenameHelper);
            var json = TestFileHelper.Read(Filename);
            _wordDictionary = _wordService.GetDictionary();
        }
        
        [Fact]
        public void WhenRequestingAVehicleByWordThenTheCategoryReturnedShouldBeVehicle()
        {
            var category = _wordService.GetCategory(Filename, "Bus");
            category.Should().Be(WordCategory.Vehicle);

        }
        
        [Fact]
        public void WhenRequestingAPlantByWordThenTheCategoryReturnedShouldBePlant()
        {
            var category = _wordService.GetCategory(Filename, "Tree");
            category.Should().Be(WordCategory.Plant);
        }
        
        [Fact]
        public void WhenRequestingAnAnimalByWordThenTheCategoryReturnedShouldBeAnimal()
        {
            var category = _wordService.GetCategory(Filename, "Elephant");
            category.Should().Be(WordCategory.Animal);
        }
        
        [Fact]
        public void WhenRequestingAnythingElseByWordThenTheCategoryReturnedShouldBeNone()
        {
            var category = _wordService.GetCategory(Filename, "Because");
            category.Should().Be(WordCategory.None);
        }

        public void Dispose()
        {
            if (File.Exists(Filename))
                File.Delete(Filename);
            
            WordDictionaryGetter.WordDictionary.Remove(Filename);
        }
    }
}