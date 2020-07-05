namespace Chat.Balderdash
{
    public class UserBalderdashGame
    {
        public int Score { get; private set; }
        public string Guess { get; private set; }
        public bool HasMadeGuessThisRound { get; private set; }


        public void SetScore(int score)
        {
            Score += score;
        }

        public void SetGuess(string guess)
        {
            Guess = guess;
        }
        
        public void MadeGuessThisRound()
        {
            HasMadeGuessThisRound = true;
        }

        public void Reset()
        {
            Guess = "";
            HasMadeGuessThisRound = false;
        }
    }
}