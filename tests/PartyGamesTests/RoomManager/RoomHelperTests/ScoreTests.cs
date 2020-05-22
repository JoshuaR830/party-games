using System.ComponentModel.DataAnnotations.Schema;
using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager.RoomHelperTests
{
    public class ScoreTests
    {
        private readonly RoomHelper _roomHelper;
        private const string name = "Joshua";
        private const string roomName = "room";

        public ScoreTests()
        {
            
            _roomHelper = new RoomHelper();
            _roomHelper.CreateRoom(name, roomName);
        }
        
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(5, 15)]
        [InlineData(10, 10)]
        [InlineData(110000010, 110000010)]
        public void WhenScoreIsSetTheUserShouldHaveTheCorrectScore(int actualScore, int expectedScore)
        {
            _roomHelper.SetScore(actualScore);
            
            Rooms.RoomsList[roomName].Users[name].Score.Should().Be(expectedScore);
        }
        
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 1, 1, 3)]
        [InlineData(5, 15, 12, 32)]
        [InlineData(1000 , 12000, 8, 13008)]
        public void WhenScoreIsUPdatedUserShouldHaveCUmulativeScore(int score1, int score2, int score3, int expectedScore)
        {
            _roomHelper.AddToScore(score1);
            _roomHelper.AddToScore(score2);
            _roomHelper.AddToScore(score3);
            
            Rooms.RoomsList[roomName].Users[name].Score.Should().Be(expectedScore);
        }
    }
}