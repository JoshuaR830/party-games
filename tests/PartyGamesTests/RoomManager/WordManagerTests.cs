using System;
using System.IO;
using Chat.RoomManager;
using FluentAssertions;
using PartyGamesTests.WordGame;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class WordManagerTests : IDisposable
    {
        private const string Filename = "./WordManger.json";
        public WordManagerTests()
        {
            TestFileHelper.Create(Filename);
        }
        
        [Fact]
        public void WhenANewWordIsCreatedThenTheWordShouldBeInitialised()
        {
            var word = new WordManager("sheep", TestFileHelper.SheepPermanentDefinition, true);
            
            word.Word.Should().Be("sheep");
            word.Definition.Should().Be(TestFileHelper.SheepPermanentDefinition);
            word.Score.Should().Be(5);
            word.IsValid.Should().BeTrue();
        }

        [Fact]
        public void WhenDefinitionIsOnlyTemporaryThenTheWordShouldBeInitialised()
        {
            var word = new WordManager("boxing", TestFileHelper.BoxingTemporaryDefinition, true);
            
            word.Word.Should().Be("boxing");
            word.Score.Should().Be(6);
            word.Definition.Should().Be(TestFileHelper.BoxingTemporaryDefinition);
            word.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordThenStatusShouldBeFalse()
        {
            var word = new WordManager("dinosaur", "", false);
            
            word.Score.Should().Be(8);
            word.Word.Should().Be("dinosaur");
            word.IsValid.Should().BeFalse();
        }

        [Fact]
        public void WhenValidityChangedTheValidityShouldBeChanged()
        {
            var word = new WordManager("dinosaur", "", false);

            word.ChangeValidity(true);
            word.IsValid.Should().BeTrue();
            word.ChangeValidity(false);
            word.IsValid.Should().BeFalse();
            word.ChangeValidity(true);
            word.IsValid.Should().BeTrue();
        }

        public void Dispose()
        {
            if(File.Exists(Filename))
                File.Delete(Filename);
        }
    }
}