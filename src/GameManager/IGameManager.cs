using Chat.RoomManager;

namespace Chat.GameManager
{
    public interface IGameManager
    {
        void SetupNewGame(string roomId, string userId, GameType game);
        void SetupNewUser(string roomId, string userId, GameThoughtsAndCrosses game);
        void ResetThoughtsAndCrosses(string roomId, GameThoughtsAndCrosses game);
        void ResetUser(string roomId, string userId, GameThoughtsAndCrosses game);
    }
}