using Chat.RoomManager;
using FluentAssertions;
using PartyGamesTests.WordGame;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class WordManagerTests
    {
        private const string Filename = "./WordManger.json";

        public WordManagerTests()
        {
            TestFileHelper.Create(Filename);
        }
        
        [Fact]
        public void WhenANewWordIsCreatedThenTheWordShouldBeInitialised()
        {
            var word = new WordManager("sheep");
            word.Word.Should().Be("sheep");
            word.Definition.Should().Be(TestFileHelper.SheepPermanentDefinition);
            word.Score.Should().Be(5);
            word.isValid.Should().BeTrue();
        }

        [Fact]
        public void WhenDefinitionIsOnlyTemporaryThenTheWordShouldBeInitialised()
        {
            var word = new WordManager("boxing");
            word.Score.Should().Be(6);
            word.Definition.Should().Be(TestFileHelper.BoxingTemporaryDefinition);
        }
        
        [Fact]
        public void WhenNotAWordThenStatusShouldBeFalse()
        {
            var word = new WordManager("dinosaur");
            word.Score.Should().Be(8);
            word.Word.Should().Be("dinosaur");
            word.isValid.Should().BeFalse();
        }
    }
}