using System;
using System.Collections.Generic;
using System.Linq;
using Chat.Balderdash;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashScoringTests
{
    public class ScoreCalculatorForDasherTests
    {
        private readonly BalderdashScoreCalculator _scoreCalculator;
        private readonly BalderdashManager _balderdashManager;
        private List<string> _notDashers;
        private string _dasher;

        public ScoreCalculatorForDasherTests()
        {
            var listOfNames = new List<string> {"Bill", "Bob", "Fred"};

            _scoreCalculator = new BalderdashScoreCalculator();
            var shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            shuffleHelper.ShuffleList(Arg.Any<List<string>>()).Returns(listOfNames);
            
            var room = new Room();
            room.SetBalderdashGame(new BalderdashManager(shuffleHelper));

            Rooms.RoomsList.TryAdd("ScoreCalculatorDasher", room);
            Rooms.RoomsList["ScoreCalculatorDasher"].Users.TryAdd(listOfNames[0], new User(listOfNames[0]));
            Rooms.RoomsList["ScoreCalculatorDasher"].Users.TryAdd(listOfNames[1], new User(listOfNames[1]));
            Rooms.RoomsList["ScoreCalculatorDasher"].Users.TryAdd(listOfNames[2], new User(listOfNames[2]));
            Rooms.RoomsList["ScoreCalculatorDasher"].SetBalderdashGame(new BalderdashManager(shuffleHelper));
            Rooms.RoomsList["ScoreCalculatorDasher"].Balderdash.AddPlayersToGame(listOfNames);
            Rooms.RoomsList["ScoreCalculatorDasher"].Balderdash.SetPlayerOrder();
            Rooms.RoomsList["ScoreCalculatorDasher"].Balderdash.SelectPlayer();
            
            Rooms.RoomsList["ScoreCalculatorDasher"].Users[listOfNames[0]].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculatorDasher"].Users[listOfNames[1]].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculatorDasher"].Users[listOfNames[2]].SetUpGame(new UserBalderdashGame());
            
            var players = Rooms.RoomsList["ScoreCalculatorDasher"].Balderdash.PlayerOrder;
            _dasher = Rooms.RoomsList["ScoreCalculatorDasher"].Balderdash.SelectedPlayer;
            _notDashers = players.Where(x => x != _dasher).ToList();
        }

        [Fact]
        public void IfDasherNotGuessedThenDasherShouldGetAScore()
        {
            Rooms.RoomsList["ScoreCalculatorDasher"].Balderdash.SetIsDasherGuessed(false);
            _scoreCalculator.CalculateDasherScore("ScoreCalculatorDasher", _dasher);

            Rooms.RoomsList["ScoreCalculatorDasher"].Users[_dasher].BalderdashGame.Score.Should().Be(2);
        }

        [Fact]
        public void IfDasherIsGuessedThenDasherShouldNotGetAScore()
        {
            Rooms.RoomsList["ScoreCalculatorDasher"].Balderdash.SetIsDasherGuessed(true);
            _scoreCalculator.CalculateDasherScore("ScoreCalculatorDasher", _dasher);
            
            Rooms.RoomsList["ScoreCalculatorDasher"].Users[_dasher].BalderdashGame.Score.Should().Be(0);
        }
    }
}