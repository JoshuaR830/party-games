using Chat.WordGame.LocalDictionaryHelpers;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.WordGame.LocalDictionaryHelpers
{
    public class FilenameHelperTests
    {
        [Fact]
        public void WhenDictionaryFilenameIsSetReturnTheDictionaryFilename()
        {
            var filenameHelper = new FilenameHelper();
            filenameHelper.SetDictionaryFilename("SomeDictionaryFile.json");
            var filename = filenameHelper.GetDictionaryFilename();

            filename.Should().Be("SomeDictionaryFile.json");
        }
        
        [Fact]
        public void WhenGuessedFilenameIsSetReturnTheGuessedFilename()
        {
            var filenameHelper = new FilenameHelper();
            filenameHelper.SetGuessedWordsFilename("SomeGuesssedFile.json");
            var filename = filenameHelper.GetGuessedWordsFilename();

            filename.Should().Be("SomeGuesssedFile.json");
        }
    }
}