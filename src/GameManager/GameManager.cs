using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;

namespace Chat.GameManager
{
    public class GameManager : IGameManager
    {
        private IJoinRoomHelper _joinRoomHelper;
        private IShuffleHelper<string> _shuffleHelper;
        private IScoreHelper _scoreHelper;
        private readonly IFilenameHelper _filenameHelper;
        private readonly IWordService _wordService;
        public GameType ActiveGameType { get; private set; }

        public GameManager(IJoinRoomHelper joinRoomHelper, IShuffleHelper<string> shuffleHelper, IScoreHelper scoreHelper, IFilenameHelper filenameHelper, IWordService wordService)
        {
            _joinRoomHelper = joinRoomHelper;
            _shuffleHelper = shuffleHelper;
            _scoreHelper = scoreHelper;
            _filenameHelper = filenameHelper;
            _wordService = wordService;
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

        public void ResetUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            var topics = Rooms.RoomsList[roomId].ThoughtsAndCrosses.Topics.ChosenTopics;

            Rooms.RoomsList[roomId].Users[userId].ThoughtsAndCrosses.CreateGrid(topics);
            var userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUserThoughtsAndCrosses(userThoughtsAndCrosses);
        }

        public void SetupNewThoughtsAndCrossesUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUserThoughtsAndCrosses(userThoughtsAndCrosses);
        }

        public void SetUpNewWordGameUser(string roomId, string userId, GameWordGame game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userWordGame = new UserWordGame(_wordService, _filenameHelper);
            Rooms.RoomsList[roomId].Users[userId].SetUserWordGame(userWordGame);
        }

        void ThoughtsAndCrosses(string roomId, string userId)
        {
            var gameThoughtsAndCrosses = new GameThoughtsAndCrosses(_shuffleHelper);
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
    }
}