using System.Collections.Generic;
using Chat.Balderdash;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashResetTests
{
    public class BalderdashUserResetTests
    {
        private readonly UserBalderdashGame _userBalderdash;

        public BalderdashUserResetTests()
        {
            var shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            shuffleHelper.ShuffleList(Arg.Any<List<string>>()).Returns(new List<string> {"Joshua"});
            
            Rooms.RoomsList.Clear();
            
            var room = new Room();
            var game = new BalderdashManager(shuffleHelper);
            room.SetBalderdashGame(game);
            Rooms.RoomsList.TryAdd("UserReset", room);

            room.Users.TryAdd("Joshua", new User("Joshua"));
            var userGame = new UserBalderdashGame();
            Rooms.RoomsList["UserReset"].Users["Joshua"].SetUpGame(userGame);

            _userBalderdash = Rooms.RoomsList["UserReset"].Users["Joshua"].BalderdashGame;
                
            _userBalderdash.SetGuess("Some guess has been made");
            _userBalderdash.SetScore(10);
            _userBalderdash.MadeGuessThisRound();
            
            Rooms.RoomsList["UserReset"].Users["Joshua"].BalderdashGame.Reset();
        }

        [Fact]
        public void WhenUserResetScoreShouldNotBeChanged()
        {
            _userBalderdash.Score.Should().Be(10);
        }
        
        [Fact]
        public void WhenUserResetHasMadeGuessThisRoundShouldBeSetToFalse()
        {
            _userBalderdash.HasMadeGuessThisRound.Should().BeFalse();
        }
        
        [Fact]
        public void WhenUserResetGuessShouldBeSetToEmptyString()
        {
            _userBalderdash.Guess.Should().Be(string.Empty);
        }
    }
}