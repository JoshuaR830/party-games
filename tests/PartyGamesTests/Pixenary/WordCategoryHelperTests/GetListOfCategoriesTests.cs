using System.Collections.Generic;
using Chat.Pixenary;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Pixenary.WordCategoryHelperTests
{
    public class GetListOfCategoriesTests
    {
        private readonly WordCategoryHelper _wordCategoryHelper;

        public GetListOfCategoriesTests(WordCategoryHelper wordCategoryHelper)
        {
            _wordCategoryHelper = wordCategoryHelper;
        }

        [Fact]
        public void ThenAListOfCategoriesExcludingNoneShouldBeReturnedShouldBeReturned()
        {
            var categories = _wordCategoryHelper.GetCategoryNames();

            categories.Should().BeEquivalentTo(new List<string> {"Animal", "Vehicle", "Plant"});
        }
    }
}