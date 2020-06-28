using System.Collections.Generic;
using Chat.Balderdash;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashResetTests
{
    public class BalderdashGameResetTests
    {
        private readonly BalderdashManager _balderdashGame;
        private readonly IShuffleHelper<string> _shuffleHelper;

        public BalderdashGameResetTests()
        {
            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            _shuffleHelper.ShuffleList(Arg.Any<List<string>>()).Returns(new List<string> {"Bill", "Bob", "Fred"});
            
            Rooms.RoomsList.Clear();
            
            var room = new Room();
            var game = new BalderdashManager(_shuffleHelper);
            room.SetBalderdashGame(game);
            Rooms.RoomsList.TryAdd("GameReset", room);

            _balderdashGame = Rooms.RoomsList["GameReset"].Balderdash;
            
            room.Users.TryAdd("Bill", new User("Bill"));
            room.Users.TryAdd("Bob", new User("Bob"));
            room.Users.TryAdd("Fred", new User("Fred"));
            
            var userGame = new UserBalderdashGame();
            Rooms.RoomsList["GameReset"].Users["Bill"].SetUpGame(userGame);
            Rooms.RoomsList["GameReset"].Users["Bob"].SetUpGame(userGame);
            Rooms.RoomsList["GameReset"].Users["Fred"].SetUpGame(userGame);
            
            _balderdashGame.SetPlayerOrder();
            _balderdashGame.SelectPlayer();
            _balderdashGame.SetIsDasherGuessed(true);

            _shuffleHelper.ClearReceivedCalls();
            Rooms.RoomsList["GameReset"].Balderdash.Reset();
        }
        
        [Fact]
        public void AfterGameResetThenPlayersShouldNotBeShuffled()
        {
            _shuffleHelper.DidNotReceive().ShuffleList(Arg.Any<List<string>>());
        }

        [Fact]
        public void AfterGameResetThenIsDasherSetShouldBeFalse()
        {
            _balderdashGame.IsDasherGuessed.Should().BeFalse();
        }

        [Fact]
        public void AfterGameResetThenADifferentPlayerShouldBeChosen()
        {
            _balderdashGame.SelectedPlayer.Should().Be("Bob");
        }
    }
}