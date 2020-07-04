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
            _scoreCalculator = new BalderdashScoreCalculator();
            var shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            shuffleHelper.ShuffleList(Arg.Any<List<string>>()).Returns(new List<string> {"Bob", "Fred", "John"});

            var room = new Room();
            room.SetBalderdashGame(new BalderdashManager(shuffleHelper));
            Rooms.RoomsList.Clear();
            Rooms.RoomsList.TryAdd("ScoreCalculator", room);
            Rooms.RoomsList["ScoreCalculator"].Users.TryAdd("Bob", new User("Bob"));
            Rooms.RoomsList["ScoreCalculator"].Users.TryAdd("Fred", new User("Fred"));
            Rooms.RoomsList["ScoreCalculator"].Users.TryAdd("John", new User("John"));
            Rooms.RoomsList["ScoreCalculator"].SetBalderdashGame(new BalderdashManager(shuffleHelper));
            Rooms.RoomsList["ScoreCalculator"].Balderdash.AddPlayersToGame(new List<string> {"Bob", "Fred", "John"});
            Rooms.RoomsList["ScoreCalculator"].Balderdash.SetPlayerOrder();
            Rooms.RoomsList["ScoreCalculator"].Balderdash.SelectPlayer();
            
            Rooms.RoomsList["ScoreCalculator"].Users["Bob"].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculator"].Users["Fred"].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList["ScoreCalculator"].Users["John"].SetUpGame(new UserBalderdashGame());
            
            var players = Rooms.RoomsList["ScoreCalculator"].Balderdash.PlayerOrder;
            _dasher = Rooms.RoomsList["ScoreCalculator"].Balderdash.SelectedPlayer;
            _notDashers = players.Where(x => x != _dasher).ToList();
        }
        
        [Fact]
        public void WhenAGuessIsMadeForAFalseProposer()
        {
            _scoreCalculator.CalculateProposer("ScoreCalculator", _notDashers[0], _notDashers[1]);
            Rooms.RoomsList["ScoreCalculator"].Users[_notDashers[1]].BalderdashGame.Score.Should().Be(1);
        }

        [Fact]
        public void WhenAGuessIsMadeForTheDasher()
        {
            _scoreCalculator.CalculateProposer("ScoreCalculator", _notDashers[0], _dasher);
            Rooms.RoomsList["ScoreCalculator"].Users[_dasher].BalderdashGame.Score.Should().Be(0);
        }
    }
}