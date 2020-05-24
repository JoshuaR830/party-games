using Chat.RoomManager;

namespace Chat.GameManager
{
    public class GameManager : IGameManager
    {
        private IJoinRoomHelper _joinRoomHelper;
        private IShuffleHelper<string> _shuffleHelper;
        private IScoreHelper _scoreHelper;

        public GameManager(IJoinRoomHelper joinRoomHelper, IShuffleHelper<string> shuffleHelper, IScoreHelper scoreHelper)
        {
            _joinRoomHelper = joinRoomHelper;
            _shuffleHelper = shuffleHelper;
            _scoreHelper = scoreHelper;
        }

        public GameType ActiveGameType { get; private set; }
        public void SetupGame(string roomId, string userId, GameType game)
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
        }

        void ThoughtsAndCrosses(string roomId, string userId)
        {
            var gameThoughtsAndCrosses = new GameThoughtsAndCrosses(_shuffleHelper);
            gameThoughtsAndCrosses.CalculateTopics();
            gameThoughtsAndCrosses.SetLetter();
            
            Rooms.RoomsList[roomId].SetThoughtsAndCrosses(gameThoughtsAndCrosses);
            
            var userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleHelper);
            userThoughtsAndCrosses.CreateGrid(gameThoughtsAndCrosses.Topics.ChosenTopics);
            
            Rooms.RoomsList[roomId].Users[userId].SetUserThoughtsAndCrosses(userThoughtsAndCrosses);
        }
    }
}