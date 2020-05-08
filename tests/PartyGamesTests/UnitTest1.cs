using System;
using Chat.Letters;
using Xunit;
using FluentAssertions;

namespace PartyGamesTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var alphabet = new Alphabet();
            alphabet.LettersList.Should().HaveCount(26);
        }
    }
}