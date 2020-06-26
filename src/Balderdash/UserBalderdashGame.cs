namespace Chat.Balderdash
{
    public class UserBalderdashGame
    {
        public int Score { get; private set; }

        public void SetScore(int score)
        {
            Score += score;
        }
    }
}