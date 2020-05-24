namespace Chat.RoomManager
{
    public class User
    {
        public string Name { get; }
        public int Score { get; private set; }

        public UserThoughtsAndCrosses UserThoughtsAndCrosses { get;}

        public User(string name)
        {
            Name = name;
            Score = 0;
        }

        public void SetScore(int score)
        {
            Score = score;
        }
        
        
    }
}