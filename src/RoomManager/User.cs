namespace Chat.RoomManager
{
    public class User
    {
        public string Name { get; }
        public int Score { get; private set; }

        public UserThoughtsAndCrosses UserThoughtsAndCrosses { get; private set; }

        public User(string name)
        {
            Name = name;
            Score = 0;
        }

        public void SetScore(int score)
        {
            Score = score;
        }

        public void SetUserThoughtsAndCrosses(UserThoughtsAndCrosses game)
        {
            UserThoughtsAndCrosses = game;
        }
    }
}