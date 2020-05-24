using Chat.GameManager;
using Chat.Letters;
using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.GameManagerTests
{
    public class GameManagerTests
    {
        [Fact]
        public void WhenGameMangerCalledANewRoomShouldBeCreated()
        {
            var gameManager = new GameManager();
            
            gameManager.SetupGame("newRoom", "Joshua", GameType.ThoughtsAndCrosses);

            Rooms.RoomsList.Should()
                .ContainKey("newRoom")
                .WhichValue
                .Should()
                .Be("Joshua");

            Rooms.RoomsList["newRoom"]
                .GameThoughtsAndCrosses
                .Should()
                .NotBeNull();

            gameManager
                .ActiveGameType
                .Should
                .Be(GameType.ThoughtsAndCrosses);
        }
    }
}