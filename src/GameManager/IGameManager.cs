namespace Chat.GameManager
{
    public interface IGameManager
    {
        void SetupGame(string roomId, string userId, GameType game);
    }
}