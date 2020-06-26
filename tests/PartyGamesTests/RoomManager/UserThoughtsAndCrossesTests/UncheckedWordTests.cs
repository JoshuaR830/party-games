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
        private UserThoughtsAndCrossesGame _userThoughtsAndCrossesGame;

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
            
            _userThoughtsAndCrossesGame = new UserThoughtsAndCrossesGame(_scoreHelper, _shuffleHelper);    
            
            _userThoughtsAndCrossesGame.CreateGrid(_categoriesInitial);
            
            _userThoughtsAndCrossesGame.CheckWord("News");
            _userThoughtsAndCrossesGame.CheckWord("Food");
            _userThoughtsAndCrossesGame.CheckWord("Fruit");
            _userThoughtsAndCrossesGame.CheckWord("Bird");
            _userThoughtsAndCrossesGame.CheckWord("Car");
            _userThoughtsAndCrossesGame.CheckWord("Animal");
            _userThoughtsAndCrossesGame.CheckWord("Boat");
            _userThoughtsAndCrossesGame.CheckWord("Plane");
            _userThoughtsAndCrossesGame.CheckWord("Colour");
        }

        [Fact]
        public void WhenAllCategoriesAreUncheckedEveryIsAcceptedShouldBeFalse()
        {
            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
            
            _userThoughtsAndCrossesGame.UncheckWord("News");
            _userThoughtsAndCrossesGame.UncheckWord("Food");
            _userThoughtsAndCrossesGame.UncheckWord("Fruit");
            _userThoughtsAndCrossesGame.UncheckWord("Bird");
            _userThoughtsAndCrossesGame.UncheckWord("Car");
            _userThoughtsAndCrossesGame.UncheckWord("Animal");
            _userThoughtsAndCrossesGame.UncheckWord("Boat");
            _userThoughtsAndCrossesGame.UncheckWord("Plane");
            _userThoughtsAndCrossesGame.UncheckWord("Colour");

            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted == false)
                .Should()
                .Be(9);
        }
        
        [Fact]
        public void WhenAllCategoriesAreUncheckedButOneEveryIsAcceptedShouldBeFalseExceptThatOne()
        {
            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
            
            _userThoughtsAndCrossesGame.UncheckWord("News");
            _userThoughtsAndCrossesGame.UncheckWord("Food");
            _userThoughtsAndCrossesGame.UncheckWord("Bird");
            _userThoughtsAndCrossesGame.UncheckWord("Car");
            _userThoughtsAndCrossesGame.UncheckWord("Animal");
            _userThoughtsAndCrossesGame.UncheckWord("Boat");
            _userThoughtsAndCrossesGame.UncheckWord("Plane");
            _userThoughtsAndCrossesGame.UncheckWord("Colour");

            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted == false)
                .Should()
                .Be(8);

            _userThoughtsAndCrossesGame
                .WordsGrid[1]
                .isAccepted
                .Should()
                .BeTrue();
        }
        
        [Fact]
        public void WhenOnlyOneCategoryIsUncheckedButOneEveryIsAcceptedThenEveryCategoryShouldBeTrueExceptThatOne()
        {
            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
            
            _userThoughtsAndCrossesGame.UncheckWord("News");

            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(8);

            _userThoughtsAndCrossesGame
                .WordsGrid[8]
                .isAccepted
                .Should()
                .BeFalse();
        }
    }
}