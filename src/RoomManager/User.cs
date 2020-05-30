using Chat.Pixenary;

namespace Chat.RoomManager
{
    public class User
    {
        public string Name { get; }
        public int Score { get; private set; }

        public UserThoughtsAndCrosses ThoughtsAndCrosses { get; private set; }
        public UserWordGame WordGame { get; private set; }
        public UserPixenaryGame PixenaryGame { get; private set; }

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
            ThoughtsAndCrosses = game;
        }
        
        public void SetUserWordGame(UserWordGame game)
        {
            WordGame = game;
        }
    }
}