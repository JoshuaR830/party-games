using System.Collections.Generic;
using Chat.Balderdash;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashResetTests
{
    public class BalderdashGamePlayerResetTests
    {
        private readonly BalderdashManager _balderdashGame;
        private readonly IShuffleHelper<string> _shuffleHelper;
        private readonly List<string> _listOfNames;

        public BalderdashGamePlayerResetTests()
        {
            this._listOfNames = new List<string> {"Bill", "Bob", "Fred"};
            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            _shuffleHelper.ShuffleList(Arg.Any<List<string>>()).Returns(new List<string> {this._listOfNames[0], this._listOfNames[1], this._listOfNames[2]});
            
            var room = new Room();
            var game = new BalderdashManager(_shuffleHelper);
            room.SetBalderdashGame(game);
            Rooms.RoomsList.TryAdd("GamePlayerReset", room);

            _balderdashGame = Rooms.RoomsList["GamePlayerReset"].Balderdash;
            
            room.Users.TryAdd(this._listOfNames[0], new User(this._listOfNames[0]));
            room.Users.TryAdd(this._listOfNames[1], new User(this._listOfNames[1]));
            room.Users.TryAdd(this._listOfNames[2], new User(this._listOfNames[2]));
            
            var userGame = new UserBalderdashGame();
            Rooms.RoomsList["GamePlayerReset"].Users[this._listOfNames[0]].SetUpGame(userGame);
            Rooms.RoomsList["GamePlayerReset"].Users[this._listOfNames[1]].SetUpGame(userGame);
            Rooms.RoomsList["GamePlayerReset"].Users[this._listOfNames[2]].SetUpGame(userGame);
            
            _balderdashGame.SetPlayerOrder();
            _balderdashGame.SelectPlayer();
            _balderdashGame.SetIsDasherGuessed(true);

            _shuffleHelper.ClearReceivedCalls();
            Rooms.RoomsList["GamePlayerReset"].Balderdash.Reset();
        }
        
        [Fact]
        public void AfterGamePlayerResetThenPlayersShouldNotBeShuffled()
        {
            _shuffleHelper.DidNotReceive().ShuffleList(Arg.Any<List<string>>());
        }

        [Fact]
        public void AfterGamePlayerResetThenIsDasherSetShouldBeFalse()
        {
            _balderdashGame.IsDasherGuessed.Should().BeFalse();
        }

        [Fact]
        public void AfterGamePlayerResetThenADifferentPlayerShouldBeChosen()
        {
            _balderdashGame.SelectedPlayer.Should().Be(_listOfNames[1]);
        }
    }
}