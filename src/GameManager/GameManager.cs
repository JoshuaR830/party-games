using Chat.RoomManager;

namespace Chat.GameManager
{
    public class GameManager : IGameManager
    {
        private IJoinRoomHelper _joinRoomHelper;
        private IShuffleHelper<string> _shuffleHelper;
        private IScoreHelper _scoreHelper;
        public GameType ActiveGameType { get; private set; }

        public GameManager(IJoinRoomHelper joinRoomHelper, IShuffleHelper<string> shuffleHelper, IScoreHelper scoreHelper)
        {
            _joinRoomHelper = joinRoomHelper;
            _shuffleHelper = shuffleHelper;
            _scoreHelper = scoreHelper;
        }
        
        public void SetupNewGame(string roomId, string userId, GameType game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            ActiveGameType = game;

            switch (ActiveGameType)
            {
                case GameType.ThoughtsAndCrosses:
                    ThoughtsAndCrosses(roomId, userId);
                    break;
                default:
                    ThoughtsAndCrosses(roomId, userId);
                    break;
            }
            
            SetupNewUser(roomId, userId, Rooms.RoomsList[roomId].GameThoughtsAndCrosses);
        }
        
        public void ResetThoughtsAndCrosses(string roomId, GameThoughtsAndCrosses game)
        {
            Rooms.RoomsList[roomId].GameThoughtsAndCrosses.SetLetter();
            Rooms.RoomsList[roomId].GameThoughtsAndCrosses.CalculateTopics();
        }

        public void ResetUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            var topics = Rooms.RoomsList[roomId].GameThoughtsAndCrosses.Topics.ChosenTopics;

            Rooms.RoomsList[roomId].Users[userId].UserThoughtsAndCrosses.CreateGrid(topics);
            var userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUserThoughtsAndCrosses(userThoughtsAndCrosses);
        }

        public void SetupNewUser(string roomId, string userId, GameThoughtsAndCrosses game)
        {
            _joinRoomHelper.CreateRoom(userId, roomId);
            var userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleHelper);
            userThoughtsAndCrosses.CreateGrid(game.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUserThoughtsAndCrosses(userThoughtsAndCrosses);
        }

        void ThoughtsAndCrosses(string roomId, string userId)
        {
            var gameThoughtsAndCrosses = new GameThoughtsAndCrosses(_shuffleHelper);
            gameThoughtsAndCrosses.CalculateTopics();
            gameThoughtsAndCrosses.SetLetter();
            
            Rooms.RoomsList[roomId].SetThoughtsAndCrosses(gameThoughtsAndCrosses);
        }
    }
}