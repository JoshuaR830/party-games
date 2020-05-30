using Chat.Pixenary;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class ChooseWordTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public ChooseWordTests()
        {
            _pixenaryManager = new PixenaryManager();
        }
        
        [Fact]
        public void WhenChooseWordIsCalledANewWordShouldBeSet()
        {
            _pixenaryManager.ChooseWord();
            _pixenaryManager.Word.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void WhenAWordIsAlreadySetAndChooseWordIsCalledThenANewWordShouldBeChosen()
        {
            _pixenaryManager.ChooseWord();
            var originalWord = _pixenaryManager.Word;
            _pixenaryManager.ChooseWord();
            var newWord = _pixenaryManager.Word;

            originalWord.Should().NotBe(newWord);
        }
    }
}