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
            var gameManager = new GameManager(new JoinRoomHelper(), new ShuffleHelper<string>(), new ScoreHelper());
            
            gameManager.SetupGame("newRoom", "Joshua", GameType.ThoughtsAndCrosses);

            Rooms
                .RoomsList
                .Should()
                .ContainKey("newRoom")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .UserThoughtsAndCrosses
                .Should()
                .NotBeNull();

            Rooms
                .RoomsList["newRoom"]
                .GameThoughtsAndCrosses
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
            gameManager.SetupGame("newRoom", "Joshua", GameType.ThoughtsAndCrosses);
            gameManager.SetupGame("newRoom", "Lydia", GameType.ThoughtsAndCrosses);
            gameManager.SetupGame("newRoom", "Kerry", GameType.ThoughtsAndCrosses);
            gameManager.SetupGame("newRoom", "Andrew", GameType.ThoughtsAndCrosses);

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
                .UserThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
            
            Rooms
                .RoomsList["newRoom"]
                .Users
                .Should()
                .ContainKey("Lydia")
                .WhichValue
                .UserThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
            
            Rooms
                .RoomsList["newRoom"]
                .Users
                .Should()
                .ContainKey("Kerry")
                .WhichValue
                .UserThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
            
            Rooms
                .RoomsList["newRoom"]
                .Users
                .Should()
                .ContainKey("Andrew")
                .WhichValue
                .UserThoughtsAndCrosses
                .WordsGrid
                .Should()
                .NotBeNull();
        }
    }
}