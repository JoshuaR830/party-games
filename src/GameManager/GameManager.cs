using Chat.Pixenary;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.GameManager
{
    public class GameManager : IGameManager
    {
        private readonly IJoinRoomHelper _joinRoomHelper;
        private readonly IShuffleHelper<string> _shuffleStringHelper;
        private readonly IShuffleHelper<WordData> _shuffleWordDataHelper;
        private readonly IScoreHelper _scoreHelper;
        private readonly IFilenameHelper _filenameHelper;
        private readonly IWordService _wordService;
        private readonly IWordCategoryHelper _wordCategoryHelper;
        public GameType ActiveGameType { get; private set; }

        public GameManager(IJoinRoomHelper joinRoomHelper, IShuffleHelper<string> shuffleStringHelper, IScoreHelper scoreHelper, IFilenameHelper filenameHelper, IWordService wordService, IShuffleHelper<WordData> shuffleWordDataHelper, IWordCategoryHelper wordCategoryHelper)
        {
            _joinRoomHelper = joinRoomHelper;
            _shuffleStringHelper = shuffleStringHelper;
            _scoreHelper = scoreHelper;
            _filenameHelper = filenameHelper;
            _wordService = wordService;
            _shuffleWordDataHelper = shuffleWordDataHelper;
            _wordCategoryHelper = wordCategoryHelper;
        }
        
        public void SetupNewGame(string roomId, string userId, GameType game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            ActiveGameType = game;

            switch (ActiveGameType)
            {
                case GameType.ThoughtsAndCrosses:
                    ThoughtsAndCrosses(roomId, userId);
                    SetupNewThoughtsAndCrossesUser(roomId, userId, Rooms.RoomsList[roomId].ThoughtsAndCrosses);
                    break;
                case GameType.WordGame:
                    WordGame(roomId, userId);
                    SetUpNewWordGameUser(roomId, userId, Rooms.RoomsList[roomId].WordGame);
                    break;
                case GameType.Pixenary:
                    
                default:
                    ThoughtsAndCrosses(roomId, userId);
                    SetupNewThoughtsAndCrossesUser(roomId, userId, Rooms.RoomsList[roomId].ThoughtsAndCrosses);
                    break;
            }
        }
        
        public void ResetThoughtsAndCrosses(string roomId, GameThoughtsAndCrosses game)
        {
            Rooms.RoomsList[roomId].ThoughtsAndCrosses.SetLetter();
            Rooms.RoomsList[roomId].ThoughtsAndCrosses.CalculateTopics();
        }

        public void ResetThoughtsAndCrosssesForUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            var topics = Rooms.RoomsList[roomId].ThoughtsAndCrosses.Topics.ChosenTopics;

            Rooms.RoomsList[roomId].Users[userId].ThoughtsAndCrosses.CreateGrid(topics);
            var userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleStringHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUserThoughtsAndCrosses(userThoughtsAndCrosses);
        }

        public void ResetWordGame(string roomId)
        {
            Rooms.RoomsList[roomId].WordGame.GetLetters();
        }
        
        public void ResetWordGameForUser(string roomId, string userId)
        {
            Rooms.RoomsList[roomId].Users[userId].WordGame.ResetWordList();
        }

        public void SetupNewThoughtsAndCrossesUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleStringHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUserThoughtsAndCrosses(userThoughtsAndCrosses);
        }

        public void SetUpNewWordGameUser(string roomId, string userId, GameWordGame game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userWordGame = new UserWordGame(_wordService, _filenameHelper);
            Rooms.RoomsList[roomId].Users[userId].SetUserWordGame(userWordGame);
        }

        public void SetUpNewPixenaryUser(string roomId, string userId, PixenaryManager game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userPixenaryGame = new UserPixenaryGame();
            Rooms.RoomsList[roomId].Users[userId].SetUserPixenaryGame(userPixenaryGame);
        }

        void ThoughtsAndCrosses(string roomId, string userId)
        {
            var gameThoughtsAndCrosses = new GameThoughtsAndCrosses(_shuffleStringHelper);
            gameThoughtsAndCrosses.CalculateTopics();
            gameThoughtsAndCrosses.SetLetter();
            
            Rooms.RoomsList[roomId].SetThoughtsAndCrosses(gameThoughtsAndCrosses);
        }
        
        void WordGame(string roomId, string userId)
        {
            var wordGame = new GameWordGame();
            wordGame.GetLetters();
            
            Rooms.RoomsList[roomId].SetWordGame(wordGame);
        }

        void PixenaryGame(string roomId, string userId)
        {
            var pixenaryGame = new PixenaryManager(_shuffleStringHelper, _shuffleWordDataHelper, _wordCategoryHelper, roomId);
            pixenaryGame.ChooseWord();
            pixenaryGame.ChooseActivePlayer();
            pixenaryGame.CreateNewList(50);
        }
    }
}