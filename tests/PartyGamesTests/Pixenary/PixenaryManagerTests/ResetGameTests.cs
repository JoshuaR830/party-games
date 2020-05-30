using Chat.Pixenary;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class ResetGameTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public ResetGameTests()
        {
            _pixenaryManager = new PixenaryManager();
            _pixenaryManager.ChooseActivePlayer();
            _pixenaryManager.ChooseWord();
            _pixenaryManager.CreateNewList(10);
            _pixenaryManager.UpdatePixel(10);
        }

        [Fact]
        public void WhenGameIsResetPlayerShouldBeReset()
        {
            _pixenaryManager.ActivePlayer.Should().NotBeNull();
            _pixenaryManager.ResetGame();
            _pixenaryManager.ActivePlayer.Should().BeNull();
        }

        [Fact]
        public void WhenGameIsResetWordShouldBeReset()
        {
            _pixenaryManager.Word.Should().NotBeNull();
            _pixenaryManager.ResetGame();
            _pixenaryManager.Word.Should().BeNull();
        }

        [Fact]
        public void WhenGameIsResetListShouldBeReset()
        {
            _pixenaryManager.Grid.Should().Contain(x => x == "");
            _pixenaryManager.ResetGame();
            _pixenaryManager.Grid.Should().OnlyContain(x => x == null);
        }

        [Fact]
        public void WhenGameIsResetPlayersAllShouldStillBePersisted()
        {
            _pixenaryManager.AddPlayer("Joshua");
            _pixenaryManager.AddPlayer("Lydia");

            _pixenaryManager.AllPlayers.Should().HaveCount(2);
            _pixenaryManager.ResetGame();
            _pixenaryManager.AllPlayers.Should().HaveCount(2);
        }
    }
}