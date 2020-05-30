using Chat.Pixenary;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.WordCategoryHelperTests
{
    public class GetAllWordsWithCategoriesTests
    {
        private readonly WordCategoryHelper _wordCategoryHelperTests;

        public GetAllWordsWithCategoriesTests()
        {
            _wordCategoryHelperTests = new WordCategoryHelper();
        }

        [Fact]
        public void OnlyWordsFromTheDictionaryWithoutCategoryNoneShouldBeReturned()
        {
            var pixenaryManager = new PixenaryManager();
            pixenaryManager.ChooseWord();

            var words = _wordCategoryHelperTests.GetAllWordsWithCategories();
            
            words.Should().NotBeEmpty().And.OnlyContain(x => x.Category != WordCategory.None);
        }
    }
}