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
        private UserThoughtsAndCrossesGame _userThoughtsAndCrossesGame;

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
            
            _userThoughtsAndCrossesGame = new UserThoughtsAndCrossesGame(_scoreHelper, _shuffleHelper);    
            
            _userThoughtsAndCrossesGame.CreateGrid(_categoriesInitial);
        }

        [Fact]
        public void WhenUserMakesAGuessUpdateTheCategoryWithTheGuess()
        {
            _userThoughtsAndCrossesGame.ManageGuess("News", "BBC news");
            _userThoughtsAndCrossesGame.ManageGuess("Food", "Banana bread");
            _userThoughtsAndCrossesGame.ManageGuess("Fruit", "Banana");
            _userThoughtsAndCrossesGame.ManageGuess("Bird", "Blue tit");
            _userThoughtsAndCrossesGame.ManageGuess("Car", "Bugatti Chiron");
            _userThoughtsAndCrossesGame.ManageGuess("Animal", "Baboon");
            _userThoughtsAndCrossesGame.ManageGuess("Boat", "Barge");
            _userThoughtsAndCrossesGame.ManageGuess("Plane", "Boeing 747");
            _userThoughtsAndCrossesGame.ManageGuess("Colour", "Blue");

            _userThoughtsAndCrossesGame.WordsGrid[0].userGuess.Should().Be("Banana bread");
            _userThoughtsAndCrossesGame.WordsGrid[1].userGuess.Should().Be("Banana");
            _userThoughtsAndCrossesGame.WordsGrid[2].userGuess.Should().Be("Blue");
            _userThoughtsAndCrossesGame.WordsGrid[3].userGuess.Should().Be("Bugatti Chiron");
            _userThoughtsAndCrossesGame.WordsGrid[4].userGuess.Should().Be("Baboon");
            _userThoughtsAndCrossesGame.WordsGrid[5].userGuess.Should().Be("Boeing 747");
            _userThoughtsAndCrossesGame.WordsGrid[6].userGuess.Should().Be("Barge");
            _userThoughtsAndCrossesGame.WordsGrid[7].userGuess.Should().Be("Blue tit");
            _userThoughtsAndCrossesGame.WordsGrid[8].userGuess.Should().Be("BBC news");
        }

        [Fact]
        public void WhenAUserChangesTheirGuessValueShouldBeUpdated()
        {
            _userThoughtsAndCrossesGame.ManageGuess("Food", "Banana");
            _userThoughtsAndCrossesGame.WordsGrid[0].userGuess.Should().Be("Banana");

            _userThoughtsAndCrossesGame.ManageGuess("Food", "Banana bread");
            _userThoughtsAndCrossesGame.WordsGrid[0].userGuess.Should().Be("Banana bread");

            _userThoughtsAndCrossesGame.ManageGuess("Food", "Bun");
            _userThoughtsAndCrossesGame.WordsGrid[0].userGuess.Should().Be("Bun");
        }
    }
}