using System;
using System.IO;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WebHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using PartyGamesTests.WordGame;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class WordManagerTests : IDisposable
    {
        private readonly IFilenameHelper _filenameHelper;
        private readonly IWordService _wordService;
        private const string Filename = "./WordManger.json";
        public WordManagerTests()
        {
            TestFileHelper.Create(Filename);
            
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _filenameHelper.GetDictionaryFilename().Returns(Filename);
            _wordService = Substitute.For<IWordService>();

        }
        
        [Fact]
        public void WhenANewWordIsCreatedThenTheWordShouldBeInitialised()
        {
            _wordService
                .GetDefinition(_filenameHelper.GetDictionaryFilename(), "sheep")
                .Returns(TestFileHelper.SheepPermanentDefinition);

            _wordService
                .GetWordStatus(_filenameHelper.GetDictionaryFilename(), "sheep")
                .Returns(true);
            
            var word = new WordManager(_wordService, _filenameHelper, "sheep");
            
            word.Word.Should().Be("sheep");
            word.Definition.Should().Be(TestFileHelper.SheepPermanentDefinition);
            word.Score.Should().Be(5);
            word.IsValid.Should().BeTrue();
        }

        [Fact]
        public void WhenDefinitionIsOnlyTemporaryThenTheWordShouldBeInitialised()
        {
            _wordService
                .GetDefinition(_filenameHelper.GetDictionaryFilename(), "boxing")
                .Returns(TestFileHelper.BoxingTemporaryDefinition);

            _wordService
                .GetWordStatus(_filenameHelper.GetDictionaryFilename(), "boxing")
                .Returns(true);
            
            var word = new WordManager(_wordService, _filenameHelper,"boxing");
            
            word.Word.Should().Be("boxing");
            word.Score.Should().Be(6);
            word.Definition.Should().Be(TestFileHelper.BoxingTemporaryDefinition);
            word.IsValid.Should().BeTrue();
        }
        
        [Fact]
        public void WhenNotAWordThenStatusShouldBeFalse()
        {
            _wordService
                .GetDefinition(_filenameHelper.GetDictionaryFilename(), "boxing")
                .Returns("");
            
            _wordService
                .GetWordStatus(_filenameHelper.GetDictionaryFilename(), "dinosaur")
                .Returns(false);
            
            var word = new WordManager(_wordService, _filenameHelper, "dinosaur");
            
            word.Score.Should().Be(8);
            word.Word.Should().Be("dinosaur");
            word.IsValid.Should().BeFalse();
        }

        [Fact]
        public void WhenValidityChangedTheValidityShouldBeChanged()
        {
            _wordService
                .GetDefinition(_filenameHelper.GetDictionaryFilename(), "boxing")
                .Returns("");
            
            _wordService
                .GetWordStatus(_filenameHelper.GetDictionaryFilename(), "dinosaur")
                .Returns(false);
            
            var word = new WordManager(_wordService, _filenameHelper, "dinosaur");

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