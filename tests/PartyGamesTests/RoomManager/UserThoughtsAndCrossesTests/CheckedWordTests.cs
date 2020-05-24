using System.Collections.Generic;
using System.Linq;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.RoomManager.UserThoughtsAndCrossesTests
{
    public class CheckedWordTests
    {
        private IShuffleHelper<string> _shuffleHelper;
        private IScoreHelper _scoreHelper;
        private readonly UserThoughtsAndCrosses _userThoughtsAndCrosses;
        private readonly List<string> _categoriesInitial;
        private List<string> _categoriesShuffled;

        public CheckedWordTests()
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
        }
        
        [Fact]
        public void WhenAllCategoriesAreCheckedEveryIsAcceptedShouldBeTrue()
        {
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

            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
        }
        
        [Fact]
        public void WhenAllCategoriesAreCheckedButOneEveryIsAcceptedShouldBeTrueExceptThatOne()
        {
            _userThoughtsAndCrosses.CreateGrid(_categoriesInitial);
            
            _userThoughtsAndCrosses.CheckWord("News");
            _userThoughtsAndCrosses.CheckWord("Food");
            _userThoughtsAndCrosses.CheckWord("Bird");
            _userThoughtsAndCrosses.CheckWord("Car");
            _userThoughtsAndCrosses.CheckWord("Animal");
            _userThoughtsAndCrosses.CheckWord("Boat");
            _userThoughtsAndCrosses.CheckWord("Plane");
            _userThoughtsAndCrosses.CheckWord("Colour");

            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(8);

            _userThoughtsAndCrosses
                .WordsGrid[1]
                .isAccepted
                .Should()
                .BeFalse();
        }
        
        [Fact]
        public void WhenOnlyOneCategoryIsCheckedButOneEveryIsNotAcceptedThenEveryCategoryShouldBeFalseExceptThatOne()
        {
            _userThoughtsAndCrosses.CreateGrid(_categoriesInitial);
            
            _userThoughtsAndCrosses.CheckWord("News");

            _userThoughtsAndCrosses
                .WordsGrid.Count(x => x.isAccepted == false)
                .Should()
                .Be(8);

            _userThoughtsAndCrosses
                .WordsGrid[8]
                .isAccepted
                .Should()
                .BeTrue();
        }
    }
}