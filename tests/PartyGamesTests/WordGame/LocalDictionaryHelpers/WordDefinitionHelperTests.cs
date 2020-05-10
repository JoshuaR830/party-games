using System.Collections.Generic;
using Chat.WordGame.LocalDictionaryHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers
{
    public class WordDefinitionHelperTests
    {
        readonly Dictionary _dictionary;

        public WordDefinitionHelperTests()
        {
            _dictionary = new Dictionary
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
                        Word = "Frog",
                        PermanentDefinition = "Irresistible kings of the pond who princesses can't resist",
                        TemporaryDefinition = null
                    },
                    new WordData
                    {
                        Word = "Cow",
                        PermanentDefinition = null,
                        TemporaryDefinition = "Makes different flavours of milk depending on diet"
                    },
                    new WordData
                    {
                        Word = "Pig",
                        PermanentDefinition = null,
                        TemporaryDefinition = null
                    }
                }
            };
        }
        
        [Fact]
        public void WhenWordHasBothPermanentAndTemporaryDefinitionsResponseShouldBePermanent()
        {
            var word = "sheep";
                
            var fileHelper = Substitute.For<IFileHelper>();
            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(_dictionary);
            
            var wordDefinitionHelper = new WordDefinitionHelper(fileHelper);
            var definitionResponse = wordDefinitionHelper.GetDefinitionForWord(word);
            definitionResponse.Should().Be("Farm animals that eat grass and have wool coats");
        }
        
        [Fact]
        public void WhenResponseOnlyHasPermanentDefinitionThenResponseShouldBePermanentDefinition()
        {
            var word = "frog";
                
            var fileHelper = Substitute.For<IFileHelper>();
            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(_dictionary);
            
            var wordDefinitionHelper = new WordDefinitionHelper(fileHelper);
            var definitionResponse = wordDefinitionHelper.GetDefinitionForWord(word);
            definitionResponse.Should().Be("Irresistible kings of the pond who princesses can't resist");
        }
        
        [Fact]
        public void WhenResponseOnlyHasTemporaryDefinitionThenResponseShouldBeTemporaryDefinition()
        {
            var word = "cow";
                
            var fileHelper = Substitute.For<IFileHelper>();
            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(_dictionary);
            
            var wordDefinitionHelper = new WordDefinitionHelper(fileHelper);
            var definitionResponse = wordDefinitionHelper.GetDefinitionForWord(word);
            definitionResponse.Should().Be("Makes different flavours of milk depending on diet");
        }
        
        [Fact]
        public void WhenResponseHasNoDefinitionThenResponseShouldBeEmptyString()
        {
            var word = "pig";
                
            var fileHelper = Substitute.For<IFileHelper>();
            fileHelper
                .ReadDictionary(Arg.Any<string>())
                .Returns(_dictionary);
            
            var wordDefinitionHelper = new WordDefinitionHelper(fileHelper);
            var definitionResponse = wordDefinitionHelper.GetDefinitionForWord(word);
            definitionResponse.Should().Be("");
        }
    }
}