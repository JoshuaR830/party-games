using System.Collections.Generic;
using System.Linq;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.RoomManager.UserThoughtsAndCrossesTests
{
    public class UncheckedWordTests
    {
        private IShuffleHelper<string> _shuffleHelper;
        private IScoreHelper _scoreHelper;
        private List<string> _categoriesInitial;
        private List<string> _categoriesShuffled;
        private UserThoughtsAndCrosses _userThoughtsAndCrosses;

        public UncheckedWordTests()
        {
            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            _scoreHelper = Substitute.For<IScoreHelper>();
            _categoriesInitial = new List<string> {"Animal", "Colour", "Bird", "News", "Food", "Boat", "Plane", "Car", "Fruit"};
            _categoriesShuffled = new List<string> {"Food", "Fruit", "Colour", "Car", "Animal", "Plane", "Boat", "Bird", "News"};

            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            _shuffleHelper
                .ShuffleList(_categoriesInitial)
                .Returns(_categoriesShuffled);
            
            _userThoughtsAndCrosses = new UserThoughtsAndCrosses(_scoreHelper, _shuffleHelper);    
            
            _userThoughtsAndCrosses.CreateGrid(_categoriesInitial);
            
            _userThoughtsAndCrosses.CheckWord("News");
            _userThoughtsAndCrosses.CheckWord("Food");
            _userThoughtsAndCrosses.CheckWord("Fruit");
            _userThoughtsAndCrosses.CheckWord("Bird");
            _userThoughtsAndCrosses.CheckWord("Car");
            _userThoughtsAndCrosses.CheckWord("Animal");
            _userThoughtsAndCrosses.CheckWord("Boat");
            _userThoughtsAndCrosses.CheckWord("Plane");
            _userThoughtsAndCrosses.CheckWord("Colour");
        }

        [Fact]
        public void WhenAllCategoriesAreUncheckedEveryIsAcceptedShouldBeFalse()
        {
            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
            
            _userThoughtsAndCrosses.UncheckWord("News");
            _userThoughtsAndCrosses.UncheckWord("Food");
            _userThoughtsAndCrosses.UncheckWord("Fruit");
            _userThoughtsAndCrosses.UncheckWord("Bird");
            _userThoughtsAndCrosses.UncheckWord("Car");
            _userThoughtsAndCrosses.UncheckWord("Animal");
            _userThoughtsAndCrosses.UncheckWord("Boat");
            _userThoughtsAndCrosses.UncheckWord("Plane");
            _userThoughtsAndCrosses.UncheckWord("Colour");

            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted == false)
                .Should()
                .Be(9);
        }
        
        [Fact]
        public void WhenAllCategoriesAreUncheckedButOneEveryIsAcceptedShouldBeFalseExceptThatOne()
        {
            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
            
            _userThoughtsAndCrosses.UncheckWord("News");
            _userThoughtsAndCrosses.UncheckWord("Food");
            _userThoughtsAndCrosses.UncheckWord("Bird");
            _userThoughtsAndCrosses.UncheckWord("Car");
            _userThoughtsAndCrosses.UncheckWord("Animal");
            _userThoughtsAndCrosses.UncheckWord("Boat");
            _userThoughtsAndCrosses.UncheckWord("Plane");
            _userThoughtsAndCrosses.UncheckWord("Colour");

            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted == false)
                .Should()
                .Be(8);

            _userThoughtsAndCrosses
                .WordsGrid[1]
                .isAccepted
                .Should()
                .BeTrue();
        }
        
        [Fact]
        public void WhenOnlyOneCategoryIsUncheckedButOneEveryIsAcceptedThenEveryCategoryShouldBeTrueExceptThatOne()
        {
            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
            
            _userThoughtsAndCrosses.UncheckWord("News");

            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(8);

            _userThoughtsAndCrosses
                .WordsGrid[8]
                .isAccepted
                .Should()
                .BeFalse();
        }
    }
}