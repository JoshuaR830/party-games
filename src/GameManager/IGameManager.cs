using Chat.Balderdash;
using Chat.Pixenary;
using Chat.RoomManager;

namespace Chat.GameManager
{
    public interface IGameManager
    {
        void SetupNewGame(string roomId, string userId, GameType game);
        void SetupNewThoughtsAndCrossesUser(string roomId, string userId, GameThoughtsAndCrosses game);
        void ResetThoughtsAndCrosses(string roomId, GameThoughtsAndCrosses game);
        void ResetThoughtsAndCrossesForUser(string roomId, string userId, GameThoughtsAndCrosses game);
        void SetUpNewWordUser(string roomId, string userId, GameWordGame game);
        void SetUpNewBalderdashUser(string roomId, string userId, BalderdashManager game);
        void SetUpNewPixenaryUser(string roomId, string userId, PixenaryManager game);
        void ResetWordGame(string roomId);
        void ResetWordGameForUser(string roomId, string userId);
    }
}