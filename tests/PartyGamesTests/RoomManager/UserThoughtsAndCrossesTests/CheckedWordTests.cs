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
        private readonly UserThoughtsAndCrossesGame _userThoughtsAndCrossesGame;
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
            
            _userThoughtsAndCrossesGame = new UserThoughtsAndCrossesGame(_scoreHelper, _shuffleHelper);    
        }
        
        [Fact]
        public void WhenAllCategoriesAreCheckedEveryIsAcceptedShouldBeTrue()
        {
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

            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(9);
        }
        
        [Fact]
        public void WhenAllCategoriesAreCheckedButOneEveryIsAcceptedShouldBeTrueExceptThatOne()
        {
            _userThoughtsAndCrossesGame.CreateGrid(_categoriesInitial);
            
            _userThoughtsAndCrossesGame.CheckWord("News");
            _userThoughtsAndCrossesGame.CheckWord("Food");
            _userThoughtsAndCrossesGame.CheckWord("Bird");
            _userThoughtsAndCrossesGame.CheckWord("Car");
            _userThoughtsAndCrossesGame.CheckWord("Animal");
            _userThoughtsAndCrossesGame.CheckWord("Boat");
            _userThoughtsAndCrossesGame.CheckWord("Plane");
            _userThoughtsAndCrossesGame.CheckWord("Colour");

            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted)
                .Should()
                .Be(8);

            _userThoughtsAndCrossesGame
                .WordsGrid[1]
                .isAccepted
                .Should()
                .BeFalse();
        }
        
        [Fact]
        public void WhenOnlyOneCategoryIsCheckedButOneEveryIsNotAcceptedThenEveryCategoryShouldBeFalseExceptThatOne()
        {
            _userThoughtsAndCrossesGame.CreateGrid(_categoriesInitial);
            
            _userThoughtsAndCrossesGame.CheckWord("News");

            _userThoughtsAndCrossesGame
                .WordsGrid.Count(x => x.isAccepted == false)
                .Should()
                .Be(8);

            _userThoughtsAndCrossesGame
                .WordsGrid[8]
                .isAccepted
                .Should()
                .BeTrue();
        }
    }
}