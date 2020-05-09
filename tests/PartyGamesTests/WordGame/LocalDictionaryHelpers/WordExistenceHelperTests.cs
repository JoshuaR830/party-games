using System.Collections.Generic;
using Chat.WordGame.LocalDictionaryHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers
{
    public class WordExistenceHelperTests
    {
        [Fact]
        public void WhenWordExistsResponseShouldBeTrue()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Farm animals that eat grass and have wool coats",
                        TemporaryDefinition = "Counted by insomniacs"
                    },
                    new WordData
                    {
                        Word = "Cow",
                        PermanentDefinition = "Farm animals - you could say the real cash cow!",
                        TemporaryDefinition = "Makes different flavours of milk depending on diet"
                    },
                    new WordData
                    {
                        Word = "Pig",
                        PermanentDefinition = "A mud loving farm animal",
                        TemporaryDefinition = "Pink animals that definitely fly"
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
        public void WhenWordDoesNotExistReturnFalse()
        {
            var word = "Zebra";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Farm animals that eat grass and have wool coats",
                        TemporaryDefinition = "Counted by insomniacs"
                    },
                    new WordData
                    {
                        Word = "Cow",
                        PermanentDefinition = "Farm animals - you could say the real cash cow!",
                        TemporaryDefinition = "Makes different flavours of milk depending on diet"
                    },
                    new WordData
                    {
                        Word = "Pig",
                        PermanentDefinition = "A mud loving farm animal",
                        TemporaryDefinition = "Pink animals that definitely fly"
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