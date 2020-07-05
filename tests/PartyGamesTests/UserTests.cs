using Chat.Pixenary;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests
{
    public class UserTests
    {
        private User user;

        public UserTests()
        {
            this.user = new User("Joshua");
        }

        [Fact]
        public void WhenTheTypeIsPixenaryThenSetUpAUserPixenaryObject()
        {
            var pixenaryGame = new UserPixenaryGame();
            user.SetUpGame(pixenaryGame);

            user.PixenaryGame.Should().NotBeNull();
        }

        [Fact]
        public void WhenTheTypeIsThoughtsAndCrossesThenSetUpAUserThoughtsAndCrossesObject()
        {
            var scoreHelper = Substitute.For<IScoreHelper>();
            var shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            
            var thoughtsAndCrosses = new UserThoughtsAndCrossesGame(scoreHelper, shuffleHelper);
            user.SetUpGame(thoughtsAndCrosses);
            user.ThoughtsAndCrossesGame.Should().NotBeNull();
        }
    }
}