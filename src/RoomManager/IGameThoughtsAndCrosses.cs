namespace Chat.RoomManager
{
    public interface IGameThoughtsAndCrosses
    {
        void SetLetter();
        void CalculateTopics();
        void SetTimer(int minutes, int seconds);
    }
}