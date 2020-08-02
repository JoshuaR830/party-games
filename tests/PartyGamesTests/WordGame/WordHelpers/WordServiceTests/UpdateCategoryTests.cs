﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amazon.Lambda;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;
namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class UpdateCategoryTests : IDisposable
    {
        private const string Filename = "./update-category-file.json";
        private WordService _wordService;
        private TemporaryDefinitionHelper _temporaryDefinitionHelper;
        
        private readonly IFilenameHelper _filenameHelper;
        private readonly FileHelper _fileHelper;
        private WordDictionary _wordDictionary;
        private WordDictionary _categoriesWordDictionary;
        private IAmazonLambda _lambda;

        public UpdateCategoryTests()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            _filenameHelper.GetGuessedWordsFilename().Returns(Filename);
            _fileHelper = new FileHelper(_filenameHelper);
            _lambda = Substitute.For<IAmazonLambda>();

            if (File.Exists(Filename))
                File.Delete(Filename);
            
            _categoriesWordDictionary = new WordDictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Elephant",
                        Category = WordCategory.None
                    },
                    new WordData
                    {
                        Word = "Tree",
                        Category = WordCategory.None
                    },
                    new WordData
                    {
                        Word = "Bus",
                        Category = WordCategory.None
                    },
                    new WordData
                    {
                        Word = "Because",
                        Category = WordCategory.Animal
                    }
                }
            };
            
            
            TestFileHelper.CreateCustomFile(Filename, _categoriesWordDictionary);

            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
            _wordDictionary = _wordService.GetDictionary();
        }
        
        [Fact]
        public void WhenSettingAVehicleCategoryTheCategoryShouldBeVehicle()
        {
            _wordService.UpdateCategory(Filename, "Bus", WordCategory.Vehicle);
            _wordDictionary
                .Words
                .Where(x => x.Word == "Bus")
                .ToList()[0]
                .Category
                .Should()
                .Be(WordCategory.Vehicle);
        }
        
        [Fact]
        public void WhenSettingAPlantCategoryTheCategoryShouldBePlant()
        {
            _wordService.UpdateCategory(Filename, "Tree", WordCategory.Plant);
            _wordDictionary
                .Words
                .Where(x => x.Word == "Tree")
                .ToList()[0]
                .Category
                .Should()
                .Be(WordCategory.Plant);
        }
        
        [Fact]
        public void WhenSettingAnAnimalCategoryTheCategoryShouldBeAnimal()
        {
            _wordService.UpdateCategory(Filename, "Elephant", WordCategory.Animal);
            _wordDictionary
                .Words
                .Where(x => x.Word == "Elephant")
                .ToList()[0]
                .Category
                .Should()
                .Be(WordCategory.Animal);
        }
        
        [Fact]
        public void WhenSettingANoneCategoryTheCategoryShouldBeNone()
        {
            _wordService.UpdateCategory(Filename, "Because", WordCategory.None);
            _wordDictionary
                .Words
                .Where(x => x.Word == "Because")
                .ToList()[0]
                .Category
                .Should()
                .Be(WordCategory.None);
        }

        public void Dispose()
        {
            if (File.Exists(Filename))
                File.Delete(Filename);
            
            WordDictionaryGetter.WordDictionary.Remove(Filename);
        }
    }
}