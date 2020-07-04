using Chat.Balderdash;
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
            
            var gameMethod = GetType().GetMethod($"{game.ToString()}");
            gameMethod?.Invoke(this, new object[] {roomId, userId});
            
            var userMethod = GetType().GetMethod($"SetUpNew{game.ToString()}User");
            var gameType = Rooms.RoomsList[roomId].GetType().GetProperty(game.ToString());
            var gameValue = gameType?.GetValue(Rooms.RoomsList[roomId]);
            userMethod?.Invoke(this, new object[] {roomId, userId, gameValue});

            // switch (ActiveGameType)
            // {
            //     case GameType.ThoughtsAndCrosses:
            //         ThoughtsAndCrosses(roomId, userId);
            //         SetupNewThoughtsAndCrossesUser(roomId, userId, Rooms.RoomsList[roomId].ThoughtsAndCrosses);
            //         break;
            //     case GameType.Word:
            //         Word(roomId, userId);
            //         SetUpNewWordUser(roomId, userId, Rooms.RoomsList[roomId].Word);
            //         break;
            //     case GameType.Pixenary:
            //         PixenaryGame(roomId, userId);
            //         SetUpNewPixenaryUser(roomId, userId, Rooms.RoomsList[roomId].PixenaryGame);
            //         break;
            //     default:
            //         ThoughtsAndCrosses(roomId, userId);
            //         SetupNewThoughtsAndCrossesUser(roomId, userId, Rooms.RoomsList[roomId].ThoughtsAndCrosses);
            //         break;
            // }
        }
        
        public void ResetThoughtsAndCrosses(string roomId, GameThoughtsAndCrosses game)
        {
            Rooms.RoomsList[roomId].ThoughtsAndCrosses.SetLetter();
            Rooms.RoomsList[roomId].ThoughtsAndCrosses.CalculateTopics();
        }

        public void ResetThoughtsAndCrossesForUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            var topics = Rooms.RoomsList[roomId].ThoughtsAndCrosses.Topics.ChosenTopics;

            Rooms.RoomsList[roomId].Users[userId].ThoughtsAndCrossesGame.CreateGrid(topics);
            var userThoughtsAndCrosses = new UserThoughtsAndCrossesGame(_scoreHelper, _shuffleStringHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUpGame(userThoughtsAndCrosses);
        }

        public void ResetWordGame(string roomId)
        {
            Rooms.RoomsList[roomId].Word.GetLetters();
        }
        
        public void ResetWordGameForUser(string roomId, string userId)
        {
            Rooms.RoomsList[roomId].Users[userId].WordGame.ResetWordList();
        }

        public void SetupNewThoughtsAndCrossesUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userThoughtsAndCrosses = new UserThoughtsAndCrossesGame(_scoreHelper, _shuffleStringHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUpGame(userThoughtsAndCrosses);
        }

        public void SetUpNewWordUser(string roomId, string userId, GameWordGame game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userWordGame = new UserWordGame(_wordService, _filenameHelper);
            Rooms.RoomsList[roomId].Users[userId].SetUpGame(userWordGame);
        }

        public void SetUpNewPixenaryUser(string roomId, string userId, PixenaryManager game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);

            var userPixenaryGame = Rooms.RoomsList[roomId].Users[userId].PixenaryGame;
            if(userPixenaryGame == null)
                userPixenaryGame = new UserPixenaryGame();
            
            Rooms.RoomsList[roomId].Users[userId].SetUpGame(userPixenaryGame);
        }

        public void SetUpNewBalderdashUser(string roomId, string userId, BalderdashManager game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);

            var userBalderdashGame = Rooms.RoomsList[roomId].Users[userId].BalderdashGame;

            if (userBalderdashGame == null)
                userBalderdashGame = new UserBalderdashGame();
            
            Rooms.RoomsList[roomId].Users[userId].SetUpGame(userBalderdashGame);
        }

        public void ThoughtsAndCrosses(string roomId, string userId)
        {
            var gameThoughtsAndCrosses = new GameThoughtsAndCrosses(_shuffleStringHelper);
            gameThoughtsAndCrosses.CalculateTopics();
            gameThoughtsAndCrosses.SetLetter();
            
            Rooms.RoomsList[roomId].SetThoughtsAndCrossesGame(gameThoughtsAndCrosses);
        }
        
        public void Word(string roomId, string userId)
        {
            var wordGame = new GameWordGame();
            wordGame.GetLetters();
            
            Rooms.RoomsList[roomId].SetWordGame(wordGame);
        }

        public void Pixenary(string roomId, string userId)
        {
            var pixenaryGame = Rooms.RoomsList[roomId].Pixenary;
            
            if(pixenaryGame == null)
                pixenaryGame = new PixenaryManager(_shuffleStringHelper, _shuffleWordDataHelper, _wordCategoryHelper, roomId);
            
            pixenaryGame.ChooseWord();
            pixenaryGame.CreateNewList(15);
            
            Rooms.RoomsList[roomId].SetPixenaryGame(pixenaryGame);
        }

        public void Balderdash(string roomId, string userId)
        {
            var balderdashGame = Rooms.RoomsList[roomId].Balderdash;

            if (balderdashGame == null)
                balderdashGame = new BalderdashManager(_shuffleStringHelper);

            Rooms.RoomsList[roomId].SetBalderdashGame(balderdashGame);
        }
    }
}