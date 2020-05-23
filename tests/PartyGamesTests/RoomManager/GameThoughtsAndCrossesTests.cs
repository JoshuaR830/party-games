using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class GameThoughtsAndCrossesTests
    {
        [Fact]
        public void WhenLetterSetIsCalledLetterShouldHaveAValue()
        {
            var thoughtsAndCrosses = new GameThoughtsAndCrosses();

            thoughtsAndCrosses.SetLetter();
            var letter1 = thoughtsAndCrosses.Letter.Letter;
            thoughtsAndCrosses.SetLetter();
            var letter2 = thoughtsAndCrosses.Letter.Letter;

            thoughtsAndCrosses
                .Letter
                .Letter
                .Should()
                .NotBeNullOrWhiteSpace();
            
            letter1.Should().NotBeEquivalentTo(letter2);
        }
        
        [Fact]
        public void SetTopics()
        {
            var thoughtsAndCrosses = new GameThoughtsAndCrosses();

            thoughtsAndCrosses.SetLetter();
            var topicsSet1 = thoughtsAndCrosses.Topics.ChosenTopics;
            thoughtsAndCrosses.SetLetter();
            var topicsSet2 = thoughtsAndCrosses.Topics.ChosenTopics;
            
            thoughtsAndCrosses
                .Topics
                .ChosenTopics
                .Should()
                .HaveCount(9);

            topicsSet1.Should().NotEqual(topicsSet2);
        }
    }
}