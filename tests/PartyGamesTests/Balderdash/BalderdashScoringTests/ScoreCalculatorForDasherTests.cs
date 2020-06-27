using Chat.Balderdash;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashScoringTests
{
    public class ScoreCalculatorForDasherTests
    {
        private readonly BalderdashScoreCalculator _scoreCalculator;
        private readonly BalderdashManager _balderdashManager;

        public ScoreCalculatorForDasherTests()
        {
            _scoreCalculator = new BalderdashScoreCalculator();
        }

        [Fact]
        public void IfDasherNotGuessedThenDasherShouldGetAScore()
        {
            _balderdashManager.SetIsDasherGuessed(false);
            _scoreCalculator.CalculateDasherScore();
        }

        [Fact]
        public void IfDasherIsGuessedThenDasherShouldNotGetAScore()
        {
            _balderdashManager.SetIsDasherGuessed(true);
            _scoreCalculator.CalculateDasherScore();   
        }
    }
}