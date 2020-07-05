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
    public class ScoreCalculatorForGuesserTests
    {
        private readonly BalderdashScoreCalculator _scoreCalculator;
        private readonly List<string> _notDashers;
        private readonly string _dasher;

        public ScoreCalculatorForGuesserTests()
        {
            var listOfNames = new List<string> {"Bill", "Bob", "Fred"};

            _scoreCalculator = new BalderdashScoreCalculator();
            var shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            shuffleHelper.ShuffleList(Arg.Any<List<string>>()).Returns(listOfNames);
            
            var room = new Room();
            room.SetBalderdashGame(new BalderdashManager(shuffleHelper));
            Rooms.RoomsList.TryAdd("ScoreCalculatorGuesser", room);
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users.TryAdd(listOfNames[0], new User(listOfNames[0]));
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users.TryAdd(listOfNames[1], new User(listOfNames[1]));
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users.TryAdd(listOfNames[2], new User(listOfNames[2]));
            Rooms.RoomsList["ScoreCalculatorGuesser"].SetBalderdashGame(new BalderdashManager(shuffleHelper));
            Rooms.RoomsList["ScoreCalculatorGuesser"].Balderdash.AddPlayersToGame(listOfNames);
            Rooms.RoomsList["ScoreCalculatorGuesser"].Balderdash.SetPlayerOrder();
            Rooms.RoomsList["ScoreCalculatorGuesser"].Balderdash.SelectPlayer();
            
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users[listOfNames[0]].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users[listOfNames[1]].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users[listOfNames[2]].SetUpGame(new UserBalderdashGame());
            
            var players = Rooms.RoomsList["ScoreCalculatorGuesser"].Balderdash.PlayerOrder;
            _dasher = Rooms.RoomsList["ScoreCalculatorGuesser"].Balderdash.SelectedPlayer;
            _notDashers = players.Where(x => x != _dasher).ToList();
        }
        
        [Fact]
        public void WhenGuesserIsCorrect()
        {
            _scoreCalculator.CalculateGuesser("ScoreCalculatorGuesser", _notDashers[0], _dasher);
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users[_notDashers[0]].BalderdashGame.Score.Should().Be(1);
        }

        [Fact]
        public void WhenGuesserIsIncorrect()
        {
            _scoreCalculator.CalculateGuesser("ScoreCalculatorGuesser", _notDashers[0], _notDashers[1]);
            Rooms.RoomsList["ScoreCalculatorGuesser"].Users[_notDashers[0]].BalderdashGame.Score.Should().Be(0);
        }
    }
}