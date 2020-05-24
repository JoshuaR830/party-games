using System.Collections.Generic;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.RoomManager.UserThoughtsAndCrossesTests
{
    public class CreateGridTests
    {
        private readonly IShuffleHelper<string> _shuffleHelper;
        private readonly UserThoughtsAndCrosses _userThoughtsAndCrosses;
        private readonly List<string> _categoriesInitial;
        private readonly List<string> _categoriesShuffled1;
        private readonly List<string> _categoriesShuffled2;


        public CreateGridTests()
        {
            _categoriesInitial = new List<string> {"Animal", "Colour", "Bird", "News", "Food", "Boat", "Plane", "Car", "Fruit"};
            _categoriesShuffled1 = new List<string> {"Food", "Fruit", "Colour", "Car", "Animal", "Plane", "Boat", "Bird", "News"};
            _categoriesShuffled2 = new List<string> { "Colour", "Fruit", "Boat", "Animal", "Plane", "Bird", "News", "Car", "Food"};
            
            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            
            _shuffleHelper
                .ShuffleList(_categoriesShuffled1)
                .Returns(_categoriesShuffled1);
            
            _shuffleHelper
                .ShuffleList(_categoriesShuffled2)
                .Returns(_categoriesShuffled2);
            
            var scoreHelper = new ScoreHelper();
            _userThoughtsAndCrosses = new UserThoughtsAndCrosses(scoreHelper, _shuffleHelper);
        }
        
        [Fact]
        public void WhenPassedAListOfCategoriesAGridShouldBeCreated()
        {
            _shuffleHelper
                .ShuffleList(_categoriesInitial)
                .Returns(_categoriesShuffled1);
            
            _userThoughtsAndCrosses.CreateGrid(_categoriesInitial);
            
            _userThoughtsAndCrosses.WordsGrid.Should().BeEquivalentTo(new List<(string category, string userGuess, bool isAccepted)>
            {
                (_categoriesShuffled1[0], "", false),
                (_categoriesShuffled1[1], "", false),
                (_categoriesShuffled1[2], "", false),
                (_categoriesShuffled1[3], "", false),
                (_categoriesShuffled1[4], "", false),
                (_categoriesShuffled1[5], "", false),
                (_categoriesShuffled1[6], "", false),
                (_categoriesShuffled1[7], "", false),
                (_categoriesShuffled1[8], "", false),
            });
        }

        [Fact]
        public void Creating2WithSameCategoriesListsShouldCreateDifferentGrids()
        {
            _userThoughtsAndCrosses.CreateGrid(_categoriesShuffled1);
            var firstGrid = _userThoughtsAndCrosses.WordsGrid;
            
            _userThoughtsAndCrosses.CreateGrid(_categoriesShuffled2);
            var secondGrid = _userThoughtsAndCrosses.WordsGrid;

            firstGrid.Should().NotEqual(secondGrid);
        }
    }
}