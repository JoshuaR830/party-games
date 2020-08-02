using System;
using System.IO;
using Chat.RoomManager;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using PartyGamesTests.WordGame;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class UserWordGameTests : IDisposable
    {
        private readonly IWordService _wordService;
        private const string Filename = "./some-file.json";

        public UserWordGameTests()
        {
            _wordService = Substitute.For<IWordService>();
        }
        
        [Fact]
        public void TestUserWordGame()
        {
            TestFileHelper.Create(Filename);
            var userGame = new UserWordGame(_wordService);

            userGame.AddWordToList("sheep");
            userGame.AddWordToList("sloth");
            userGame.AddWordToList("dinosaur");
            
            userGame.WordList.Should().HaveCount(3);
        }
        
        public void Dispose()
        {
            if(File.Exists(Filename))
                File.Delete(Filename);
        }
    }
}