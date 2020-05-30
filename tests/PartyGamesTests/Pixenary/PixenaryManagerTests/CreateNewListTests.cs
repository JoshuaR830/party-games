using Chat.Pixenary;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class CreateNewListTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public CreateNewListTests()
        {
            _pixenaryManager = new PixenaryManager();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(10, 100)]
        [InlineData(20, 400)]
        [InlineData(100, 10000)]
        public void WhenTheListIsCreatedThereShouldBe2500Elements(int gridSize, int expectedCellNumber)
        {
            _pixenaryManager.CreateNewList(gridSize);
            var actualCellNumber = _pixenaryManager.Grid.Count;
            actualCellNumber.Should().Be(expectedCellNumber);
        }

        [Fact]
        public void WhenTheListIsCreatedAllElementsShouldBeNull()
        {
            _pixenaryManager.CreateNewList(10);
            var cells = _pixenaryManager.Grid;

            cells.Should().OnlyContain(x => x == null);
        }
    }
}