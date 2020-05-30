using Chat.Pixenary;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class ChoosePlayerTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public ChoosePlayerTests()
        {
            _pixenaryManager = new PixenaryManager();
            _pixenaryManager.AddPlayer("Joshua");
            _pixenaryManager.AddPlayer("Lydia");
        }

        [Fact]
        public void WhenAPlayerIsChosenThenAValueShouldBeSet()
        {
            _pixenaryManager.ChooseActivePlayer();
            _pixenaryManager.ActivePlayer.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void WhenAPlayerIsUpdatedADifferentPlayerShouldBeChosen()
        {
            _pixenaryManager.ChooseActivePlayer();
            var player1 = _pixenaryManager.ActivePlayer;
            _pixenaryManager.ChooseActivePlayer();
            var player2 = _pixenaryManager.ActivePlayer;
            player1.Should().NotBe(player2);
        }
    }
}