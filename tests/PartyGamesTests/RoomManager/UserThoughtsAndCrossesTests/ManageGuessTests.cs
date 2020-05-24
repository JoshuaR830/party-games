using System.Collections.Generic;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.RoomManager.UserThoughtsAndCrossesTests
{
    public class ManageGuessTests
    {
        private IShuffleHelper<string> _shuffleHelper;
        private IScoreHelper _scoreHelper;
        private List<string> _categoriesInitial;
        private List<string> _categoriesShuffled;
        private UserThoughtsAndCrosses _userThoughtsAndCrosses;

        public ManageGuessTests()
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
        }

        [Fact]
        public void WhenUserMakesAGuessUpdateTheCategoryWithTheGuess()
        {
            _userThoughtsAndCrosses.ManageGuess("News", "BBC news");
            _userThoughtsAndCrosses.ManageGuess("Food", "Banana bread");
            _userThoughtsAndCrosses.ManageGuess("Fruit", "Banana");
            _userThoughtsAndCrosses.ManageGuess("Bird", "Blue tit");
            _userThoughtsAndCrosses.ManageGuess("Car", "Bugatti Chiron");
            _userThoughtsAndCrosses.ManageGuess("Animal", "Baboon");
            _userThoughtsAndCrosses.ManageGuess("Boat", "Barge");
            _userThoughtsAndCrosses.ManageGuess("Plane", "Boeing 747");
            _userThoughtsAndCrosses.ManageGuess("Colour", "Blue");

            _userThoughtsAndCrosses.WordsGrid[0].userGuess.Should().Be("Banana bread");
            _userThoughtsAndCrosses.WordsGrid[1].userGuess.Should().Be("Banana");
            _userThoughtsAndCrosses.WordsGrid[2].userGuess.Should().Be("Blue");
            _userThoughtsAndCrosses.WordsGrid[3].userGuess.Should().Be("Bugatti Chiron");
            _userThoughtsAndCrosses.WordsGrid[4].userGuess.Should().Be("Baboon");
            _userThoughtsAndCrosses.WordsGrid[5].userGuess.Should().Be("Boeing 747");
            _userThoughtsAndCrosses.WordsGrid[6].userGuess.Should().Be("Barge");
            _userThoughtsAndCrosses.WordsGrid[7].userGuess.Should().Be("Blue tit");
            _userThoughtsAndCrosses.WordsGrid[8].userGuess.Should().Be("BBC news");
        }

        [Fact]
        public void WhenAUserChangesTheirGuessValueShouldBeUpdated()
        {
            _userThoughtsAndCrosses.ManageGuess("Food", "Banana");
            _userThoughtsAndCrosses.ManageGuess("Food", "Banana Bread");
            _userThoughtsAndCrosses.ManageGuess("Food", "Bun");
            
            _userThoughtsAndCrosses.WordsGrid[0].userGuess.Should().Be("Bun");
        }
    }
}