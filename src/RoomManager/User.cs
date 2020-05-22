namespace Chat.RoomManager
{
    public class User
    {
        public string Name { get; }
        public int Score { get; }

        public User(string name)
        {
            Name = name;
            Score = 0;
        }
        
        
    }
}