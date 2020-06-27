using Chat.Balderdash;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashPlayerTests
{
    public class ScoreTests
    {
        private readonly UserBalderdashGame _balderdash;

        public ScoreTests()
        {
            _balderdash = new UserBalderdashGame();
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(5, 10, 20, 35)]
        [InlineData(500, 0, 0, 500)]
        public void ThenThePlayersScoreShouldBeUpdated(int input1, int input2, int input3, int expected)
        {
            _balderdash.SetScore(input1);
            _balderdash.SetScore(input2);
            _balderdash.SetScore(input3);
            _balderdash.Score.Should().Be(expected);
        }
    }
}