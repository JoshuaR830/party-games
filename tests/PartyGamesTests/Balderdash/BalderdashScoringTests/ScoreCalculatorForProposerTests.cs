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
    public class ScoreCalculatorForProposerTests
    {
        private readonly BalderdashScoreCalculator _scoreCalculator;
        private readonly List<string> _notDashers;
        private readonly string _dasher;

        public ScoreCalculatorForProposerTests()
        {
            var listOfNames = new List<string> {"Bill", "Bob", "Fred"};

            _scoreCalculator = new BalderdashScoreCalculator();
            var shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            shuffleHelper.ShuffleList(Arg.Any<List<string>>()).Returns(listOfNames);

            var room = new Room();
            room.SetBalderdashGame(new BalderdashManager(shuffleHelper));

            Rooms.RoomsList.TryAdd("ScoreCalculatorProposer", room);
            Rooms.RoomsList["ScoreCalculatorProposer"].Users.TryAdd(listOfNames[0], new User(listOfNames[0]));
            Rooms.RoomsList["ScoreCalculatorProposer"].Users.TryAdd(listOfNames[1], new User(listOfNames[1]));
            Rooms.RoomsList["ScoreCalculatorProposer"].Users.TryAdd(listOfNames[2], new User(listOfNames[2]));
            Rooms.RoomsList["ScoreCalculatorProposer"].SetBalderdashGame(new BalderdashManager(shuffleHelper));
            Rooms.RoomsList["ScoreCalculatorProposer"].Balderdash.AddPlayersToGame(listOfNames);
            Rooms.RoomsList["ScoreCalculatorProposer"].Balderdash.SetPlayerOrder();
            Rooms.RoomsList["ScoreCalculatorProposer"].Balderdash.SelectPlayer();
            
            Rooms.RoomsList["ScoreCalculatorProposer"].Users[listOfNames[0]].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculatorProposer"].Users[listOfNames[1]].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculatorProposer"].Users[listOfNames[2]].SetUpGame(new UserBalderdashGame());
            
            var players = Rooms.RoomsList["ScoreCalculatorProposer"].Balderdash.PlayerOrder;
            _dasher = Rooms.RoomsList["ScoreCalculatorProposer"].Balderdash.SelectedPlayer;
            _notDashers = players.Where(x => x != _dasher).ToList();
        }
        
        [Fact]
        public void WhenAGuessIsMadeForAFalseProposer()
        {
            _scoreCalculator.CalculateProposer("ScoreCalculatorProposer", _notDashers[0], _notDashers[1]);
            Rooms.RoomsList["ScoreCalculatorProposer"].Users[_notDashers[1]].BalderdashGame.Score.Should().Be(1);
        }

        [Fact]
        public void WhenAGuessIsMadeForTheDasher()
        {
            _scoreCalculator.CalculateProposer("ScoreCalculatorProposer", _notDashers[0], _dasher);
            Rooms.RoomsList["ScoreCalculatorProposer"].Users[_dasher].BalderdashGame.Score.Should().Be(0);
        }
    }
}