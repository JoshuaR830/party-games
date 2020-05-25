using Chat.GameManager;
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
            var gameManager = new GameManager(new JoinRoomHelper(), new ShuffleHelper<string>(), new ScoreHelper());
            
            gameManager.SetupNewGame("newRoom", "Joshua", GameType.ThoughtsAndCrosses);

            Rooms
                .RoomsList
                .Should()
                .ContainKey("newRoom")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .ThoughtsAndCrosses
                .Should()
                .NotBeNull();

            Rooms
                .RoomsList["newRoom"]
                .ThoughtsAndCrosses
                .Should()
                .NotBeNull();

            gameManager
                .ActiveGameType
                .Should()
                .Be(GameType.ThoughtsAndCrosses);
        }

        [Fact]
        public void WhenMultipleUsersAreInARoomThenTheyShouldAllBeInitialised()
        {
            var gameManager = new GameManager(new JoinRoomHelper(), new ShuffleHelper<string>(), new ScoreHelper());
            gameManager.SetupNewGame("newRoom", "Joshua", GameType.ThoughtsAndCrosses);
            gameManager.SetupNewGame("newRoom", "Lydia", GameType.ThoughtsAndCrosses);
            gameManager.SetupNewGame("newRoom", "Kerry", GameType.ThoughtsAndCrosses);
            gameManager.SetupNewGame("newRoom", "Andrew", GameType.ThoughtsAndCrosses);

            Rooms.RoomsList["newRoom"]
                .Users
                .Count
                .Should()
                .Be(4);
            
            Rooms
                .RoomsList["newRoom"]
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .ThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
            
            Rooms
                .RoomsList["newRoom"]
                .Users
                .Should()
                .ContainKey("Lydia")
                .WhichValue
                .ThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
            
            Rooms
                .RoomsList["newRoom"]
                .Users
                .Should()
                .ContainKey("Kerry")
                .WhichValue
                .ThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
            
            Rooms
                .RoomsList["newRoom"]
                .Users
                .Should()
                .ContainKey("Andrew")
                .WhichValue
                .ThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
        }
    }
}