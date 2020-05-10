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
        public void WhenWordDoesNotExistResponseShouldBeFalse()
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
        
        [Fact]
        public void WhenWordExistsButTheDefinitionIsCompletelyObsoleteResponseShouldBeFalse()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Cow",
                        PermanentDefinition = "Farm animals - you could say the real cash cow!",
                        TemporaryDefinition = "Makes different flavours of milk depending on diet"
                    },
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "1. Something something [Obs.]; Something [Archaic] \n Something else [Scot.] 2. Something else [Irish]",
                        TemporaryDefinition = "1. Something something [Obs.]; Something [Archaic] \n Something else [Scot.] 2. Something else [Irish]"
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
        
        [Fact]
        public void WhenWordDefinitionIsObsoleteResponseShouldBeFalse()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Something something [Obs.]",
                        TemporaryDefinition = "Something something [Obs.]"
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
        
        [Fact]
        public void WhenWordDefinitionIsArchaicResponseShouldBeFalse()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Something [Archaic]",
                        TemporaryDefinition = "Something [Archaic]"
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
        
        [Fact]
        public void WhenWordDefinitionIsScottishResponseShouldBeFalse()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Something else [Scot.]",
                        TemporaryDefinition = "Something else [Scot.]"
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
        
        [Fact]
        public void WhenWordDefinitionIsIrishResponseShouldBeFalse()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Something else [Irish]",
                        TemporaryDefinition = "Something else [Irish]"
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
        
        [Fact]
        public void WhenWordExistsButHasBadDataOnOneNewLineObsoleteResponseShouldBeTrue()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Something something [Obs.]\n A woolly farm animal;",
                        TemporaryDefinition = "Something something [Obs.]\n A woolly farm animal;"
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
        public void WhenWordExistsButHasBadDataInPartOfTheNumberedListResponseShouldBeTrue()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "1. Something something [Obs.] 2. A woolly farm animal;",
                        TemporaryDefinition = "1. Something something [Obs.] 2. A woolly farm animal;"
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
        public void WhenWordExistsButHasBadDataInPartOfTheSemiColonListResponseShouldBeTrue()
        {
            var word = "Sheep";
            var dictionary = new Dictionary
            {
                Words = new List<WordData>
                {
                    new WordData
                    {
                        Word = "Sheep",
                        PermanentDefinition = "Something something [Obs.]; A woolly farm animal;",
                        TemporaryDefinition = "Something something [Obs.]; A woolly farm animal;"
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
    }
}