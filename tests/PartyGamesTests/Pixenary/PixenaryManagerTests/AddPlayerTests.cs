using Chat.Pixenary;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class AddPlayerTests
    {
        private PixenaryManager _pixenaryManager;

        public AddPlayerTests()
        {
            _pixenaryManager = new PixenaryManager();
        }
        
        [Fact]
        public void WhenAPlayerJoinsTheGameTheyShouldBeAddedToTheList()
        {
            _pixenaryManager.AddPlayer("Andrew");
            _pixenaryManager.AddPlayer("Joshua");
            _pixenaryManager.AddPlayer("Lydia");
            _pixenaryManager.AddPlayer("Kerry");

            _pixenaryManager.AllPlayers.Should().HaveCount(4);
        }
    }
}