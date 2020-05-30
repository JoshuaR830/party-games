using Chat.GameManager;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.GameManagerTests
{
    public class GameManagerTests
    {
        private IFilenameHelper _filenameHelper;
        private IWordService _wordService;

        [Fact]
        public void WhenGameMangerCalledANewRoomShouldBeCreated()
        {
            _filenameHelper = Substitute.For<IFilenameHelper>();
            _wordService = Substitute.For<IWordService>();
            
            var gameManager = new GameManager(new JoinRoomHelper(), new ShuffleHelper<string>(), new ScoreHelper(), _filenameHelper, _wordService);
            
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
            var gameManager = new GameManager(new JoinRoomHelper(), new ShuffleHelper<string>(), new ScoreHelper(), _filenameHelper, _wordService);
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