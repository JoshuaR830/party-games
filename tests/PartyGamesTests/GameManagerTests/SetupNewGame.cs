using System.Collections.Generic;
using Chat.GameManager;
using Chat.Pixenary;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.GameManagerTests
{
    public class SetupNewGame
    {
        private GameManager _gameManager;
        // private IJoinRoomHelper joinRoomHelper;
        private IShuffleHelper<string> shuffleStringHelper;
        private IScoreHelper scoreHelper;
        private IFilenameHelper filenameHelper;
        private IWordService wordService;
        private IShuffleHelper<WordData> shuffleWordDataHelper;
        private IWordCategoryHelper wordCategoryHelper;
        private readonly JoinRoomHelper _joinRoomHelper;
        private string roomId;
        private string userId;

        public SetupNewGame()
        {
            _joinRoomHelper = new JoinRoomHelper();
            
            // joinRoomHelper = Substitute.For<IJoinRoomHelper>();
            shuffleStringHelper = Substitute.For<IShuffleHelper<string>>();
            scoreHelper = Substitute.For<IScoreHelper>();
            filenameHelper = Substitute.For<IFilenameHelper>();
            wordService = Substitute.For<IWordService>();
            shuffleWordDataHelper = Substitute.For<IShuffleHelper<WordData>>();
            wordCategoryHelper = Substitute.For<IWordCategoryHelper>();

            this.userId = "Joshua";
            this.roomId = "GroupOfJoshua";
            
            _joinRoomHelper.CreateRoom(userId, roomId);

            shuffleStringHelper.ShuffleList(Arg.Any<List<string>>()).Returns(new List<string>
            {
                "Topic1", "Topic2", "Topic3", "Topic4", "Topic5", "Topic6", "Topic7", "Topic8", "Topic9"
            });

            shuffleWordDataHelper.ShuffleList(Arg.Any<List<WordData>>()).Returns(new List<WordData>
            {
                new WordData
                {
                    Word = "Hi"
                },
                new WordData
                {
                    Word = "Hello"
                },
            });
            _gameManager = new GameManager(_joinRoomHelper, shuffleStringHelper, scoreHelper, filenameHelper, wordService, shuffleWordDataHelper, wordCategoryHelper);
        }

        [Fact]
        public void WhenTheTypeIsThoughtsAndCrossesThenThoughtsAndCrossesGameShouldNotBeNull()
        {
            _gameManager.SetupNewGame(roomId, userId, GameType.ThoughtsAndCrosses);
            Rooms.RoomsList[roomId].ThoughtsAndCrosses.Should().NotBeNull();
        }
        
        [Fact]
        public void WhenTheTypeIsPixenaryThenPixenaryGameShouldNotBeNull()
        {
            _gameManager.SetupNewGame(roomId, userId, GameType.Pixenary);
            Rooms.RoomsList[roomId].Pixenary.Should().NotBeNull();
        }
        
        [Fact]
        public void WhenTheTypeIsBalderdashThenBaderdashGameShouldNotBeNull()
        {
            _gameManager.SetupNewGame(roomId, userId, GameType.Balderdash);
            Rooms.RoomsList[roomId].Balderdash.Should().NotBeNull();
        }
        
    }
}