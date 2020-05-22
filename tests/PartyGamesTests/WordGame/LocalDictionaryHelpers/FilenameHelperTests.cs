using Chat.WordGame.LocalDictionaryHelpers;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers
{
    public class FilenameHelperTests
    {
        [Fact]
        public void WhenFilenameIsSetReturnTheFilename()
        {
            var filenameHelper = new FilenameHelper();
            filenameHelper.SetDictionaryFilename("SomeFile.json");
            var filename = filenameHelper.GetDictionaryFilename();

            filename.Should().Be("SomeFile.json");
        }
    }
}