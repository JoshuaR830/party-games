namespace Chat.RoomManager
{
    public interface IRoomHelper
    {
        void CreateRoom(string name, string roomId);
        void AddToScore(int scoreToAdd);
        void SetScore(int newScore);
    }
}