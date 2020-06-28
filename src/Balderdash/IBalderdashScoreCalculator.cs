namespace Chat.Balderdash
{
    public interface IBalderdashScoreCalculator
    {
        void CalculateGuesser(string roomId, string playerWhoGuessed, string playerWhoProposed);
        void CalculateProposer(string roomId, string playerWhoGuessed, string playerWhoProposed);
        void CalculateDasherScore(string roomId, string dasher);
    }
}