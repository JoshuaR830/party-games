using System;
using System.Collections.Generic;
using System.IO;
using Chat.Pixenary;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.Pixenary.WordCategoryHelperTests
{
    public class GetAllWordsWithCategoriesTests
    {
        private readonly WordCategoryHelper _wordCategoryHelper;
        private const string Filename = "./categories-test";

        public GetAllWordsWithCategoriesTests()
        {
            var fileHelper = Substitute.For<IFileHelper>();
            var filenameHelper = Substitute.For<IFilenameHelper>();

            filenameHelper
                .GetDictionaryFilename()
                .Returns(Filename);

            fileHelper.ReadDictionary(Filename).Returns(new WordDictionary
            {
                Words = new List<WordData>
                {
                    new WordData { Category = WordCategory.Animal },
                    new WordData { Category = WordCategory.Plant },
                    new WordData { Category = WordCategory.Vehicle },
                    new WordData { Category = WordCategory.None },
                    new WordData { Category = WordCategory.None },
                    new WordData { Category = WordCategory.Animal },
                    new WordData { Category = WordCategory.Plant },
                    new WordData { Category = WordCategory.Vehicle },
                    new WordData { Category = WordCategory.None }
                },
            });

            _wordCategoryHelper = new WordCategoryHelper(fileHelper, filenameHelper);
        }

        [Fact]
        public void OnlyWordsFromTheDictionaryWithoutCategoryNoneShouldBeReturned()
        {
            var words = _wordCategoryHelper.GetAllWordsWithCategories();
            
            words.Should().NotBeEmpty().And.OnlyContain(x => x.Category != WordCategory.None);
        }
    }
}