namespace Chat.Pixenary
{
    public class UserPixenaryGame
    {
        public int Score { get; private set; }

        public void SetScore(int score)
        {
            Score += score;
        }
    }
}