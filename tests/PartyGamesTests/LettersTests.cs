using Chat.Letters;
using Xunit;
using FluentAssertions;

namespace PartyGamesTests
{
    public class LettersTests
    {
        [Fact]
        public void WhenAlphabetIsCreatedLettersListShouldContain26Letters()
        {
            var alphabet = new Alphabet();
            alphabet.LettersList.Should().HaveCount(26);
        }
    }
}