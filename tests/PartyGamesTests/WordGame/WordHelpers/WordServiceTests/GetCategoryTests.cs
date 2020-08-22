using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.WordHelpers.WordServiceTests
{
    public class GetCategoryTests : IDisposable
    {
        private const string Filename = "./add-new-category-to-file.json";
        private WordService _wordService;
        private TemporaryDefinitionHelper _temporaryDefinitionHelper;
        
        private readonly IFilenameHelper _filenameHelper;
        private readonly FileHelper _fileHelper;
        private WordDictionary _categoriesWordDictionary;
        private IAmazonLambda _lambda;

        public GetCategoryTests()
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
            
            var wordResponse = new WordResponseWrapper
            {
                IsSuccessful = true,
                WordResponse = null
            };
            
            var json = JsonConvert.SerializeObject(wordResponse);

            var jsonBytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream();
            stream.Write(jsonBytes, 0, jsonBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            
            _lambda.InvokeAsync(Arg.Any<InvokeRequest>()).Returns(new InvokeResponse
            {
                Payload = stream
            });
            
            TestFileHelper.CreateCustomFile(Filename, _categoriesWordDictionary);

            _wordService = new WordService(_fileHelper, _filenameHelper, _lambda);
        }
        
        [Fact]
        public async Task WhenRequestingAVehicleByWordThenTheCategoryReturnedShouldBeVehicle()
        {
            var category = await _wordService.GetCategory("Bus");
            category.Should().Be(WordCategory.Vehicle);
        }
        
        [Fact]
        public async Task WhenRequestingAPlantByWordThenTheCategoryReturnedShouldBePlant()
        {
            var category = await _wordService.GetCategory("Tree");
            category.Should().Be(WordCategory.Plant);
        }
        
        [Fact]
        public async Task WhenRequestingAnAnimalByWordThenTheCategoryReturnedShouldBeAnimal()
        {
            var category = await _wordService.GetCategory("Elephant");
            category.Should().Be(WordCategory.Animal);
        }
        
        [Fact]
        public async Task WhenRequestingAnythingElseByWordThenTheCategoryReturnedShouldBeNone()
        {
            var category = await _wordService.GetCategory("Because");
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