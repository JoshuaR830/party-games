using System;
using System.Collections.Generic;
using System.IO;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers
{
    public class WordExistenceHelperWithStatusesTest
    {
        [Fact]
        public void WhenWordExistsInDictionaryWithStatusPermanentThenExistenceShouldBeTrue()
        {
            var word = "Sheep";
            var dictionary = new WordDictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Some definition",
                        TemporaryDefinition = "Some definition",
                        Status = WordStatus.Permanent
                    }
                }
            };
            
            var fileHelper = Substitute.For<IFileHelper>();

            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(dictionary);
            
            var wordExistenceHelper = new WordExistenceHelper(fileHelper);
            var response = wordExistenceHelper.DoesWordExist(word);
            response.Should().BeTrue();
        }
        
        [Fact]
        public void WhenWordExistsInDictionaryWithStatusTemporaryThenExistenceShouldBeTrue()
        {
            var word = "Sheep";
            var dictionary = new WordDictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Some definition",
                        TemporaryDefinition = "Some definition",
                        Status = WordStatus.Temporary
                    }
                }
            };
            
            var fileHelper = Substitute.For<IFileHelper>();

            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(dictionary);
            
            var wordExistenceHelper = new WordExistenceHelper(fileHelper);
            var response = wordExistenceHelper.DoesWordExist(word);
            response.Should().BeTrue();
        }

        [Fact]
        public void WhenWordExistsInDictionaryWithStatusSuffixThenExistenceShouldBeTrue()
        {
            var word = "Sheep";
            var dictionary = new WordDictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Some definition",
                        TemporaryDefinition = "Some definition",
                        Status = WordStatus.Suffix
                    }
                }
            };
            
            var fileHelper = Substitute.For<IFileHelper>();

            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(dictionary);
            
            var wordExistenceHelper = new WordExistenceHelper(fileHelper);
            var response = wordExistenceHelper.DoesWordExist(word);
            response.Should().BeTrue();
        }

        [Fact]
        public void WhenWordExistsInDictionaryWithStatusDoesNotExistThenExistenceShouldBeFalse()
        {
            var word = "Sheep";
            var dictionary = new WordDictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Some definition",
                        TemporaryDefinition = "Some definition",
                        Status = WordStatus.DoesNotExist
                    }
                }
            };
            
            var fileHelper = Substitute.For<IFileHelper>();

            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(dictionary);
            
            var wordExistenceHelper = new WordExistenceHelper(fileHelper);
            var response = wordExistenceHelper.DoesWordExist(word);
            response.Should().BeFalse();
        }
    }
}