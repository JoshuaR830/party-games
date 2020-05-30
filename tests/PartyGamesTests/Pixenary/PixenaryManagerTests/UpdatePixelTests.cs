using Chat.Pixenary;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class UpdatePixelTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public UpdatePixelTests()
        {
            _pixenaryManager = new PixenaryManager();
            _pixenaryManager.CreateNewList(10);
        }

        [Theory]
        [InlineData(0, "#ff00ff")]
        [InlineData(0, "#ffffff")]
        [InlineData(1, "#ffaaff")]
        [InlineData(1, "#ff11ff")]
        [InlineData(10, "#ffffff")]
        [InlineData(10, "#ff11ff")]
        [InlineData(11, "#ff00ff")]
        [InlineData(11, "#0000ff")]
        [InlineData(99, "#1100ff")]
        [InlineData(99, "#4400ff")]
        public void WhenAPixelLocationIsSetThenTheListShouldBeUpdated(int pixel, string colour)
        {
            _pixenaryManager.UpdatePixel(pixel);
            _pixenaryManager.Grid[pixel].Should().Be(colour);
        }
    }
}