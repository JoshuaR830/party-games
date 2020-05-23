using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.RoomManager.GameThoughtsAndCrossesTests
{
    public class SetTimerTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 20, 80)]
        [InlineData(0, 20, 20)]
        [InlineData(1, 0, 60)]
        [InlineData(59, 59, 3540)]
        public void WhenMinutesAndSecondsSetTheTotalSecondsShouldBeReturned(int minutes, int seconds, int expectedTotal)
        {
            IShuffleHelper<string> shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            var gameThoughtsAndCrosses = new GameThoughtsAndCrosses(shuffleHelper);
            gameThoughtsAndCrosses.SetTimer(minutes, seconds);
            
            gameThoughtsAndCrosses
                .TimerSeconds
                .Should()
                .Be(expectedTotal);
        }
    }
}