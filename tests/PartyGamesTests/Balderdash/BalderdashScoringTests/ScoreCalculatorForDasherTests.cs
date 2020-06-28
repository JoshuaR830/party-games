using System.Collections.Generic;
using System.Linq;
using Chat.Balderdash;
using Chat.RoomManager;
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
            
            var players = Rooms.RoomsList["ScoreCalculator"].Balderdash.PlayerOrder;
            _dasher = Rooms.RoomsList["ScoreCalculator"].Balderdash.SelectedPlayer;
            _notDashers = players.Where(x => x != _dasher).ToList();
        }

        [Fact]
        public void IfDasherNotGuessedThenDasherShouldGetAScore()
        {
            Rooms.RoomsList["ScoreCalculator"].Balderdash.SetIsDasherGuessed(false);
            _scoreCalculator.CalculateDasherScore("ScoreCalculator", _dasher);
        }

        [Fact]
        public void IfDasherIsGuessedThenDasherShouldNotGetAScore()
        {
            Rooms.RoomsList["ScoreCalculator"].Balderdash.SetIsDasherGuessed(true);
            _scoreCalculator.CalculateDasherScore("ScoreCalculator", _dasher);   
        }
    }
}