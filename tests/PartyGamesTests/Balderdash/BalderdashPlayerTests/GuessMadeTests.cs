using Chat.Balderdash;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashPlayerTests
{
    public class GuessMadeTests
    {
        private UserBalderdashGame _balderdash;

        public GuessMadeTests()
        {
            _balderdash = new UserBalderdashGame();
            _balderdash.SetGuess("Hello");
        }

        [Fact]
        public void CheckThatPreviousGuessIsSaved()
        {
            _balderdash.Guess.Should().Be("Hello");
        }
    }
}