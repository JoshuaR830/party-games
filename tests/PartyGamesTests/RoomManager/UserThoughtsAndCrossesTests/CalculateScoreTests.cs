using System.Collections.Generic;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.RoomManager.UserThoughtsAndCrossesTests
{
    public class CalculateScoreTests
    {
        private IShuffleHelper<string> _shuffleHelper;
        
        [Fact]
        public void WhenCategoriesSelectedScoreShouldBeCalculatedCorrectly()
        {
           var userThoughtsAndCrosses = new UserThoughtsAndCrosses();

            userThoughtsAndCrosses.CalculateScore();
            var initialScore = userThoughtsAndCrosses.Score;
            
            var categories = new List<string> {"Animal", "Colour", "Bird", "News", "Food", "Boat", "Plane", "Car", "Fruit"};

            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            _shuffleHelper
                .ShuffleList(categories)
                .Returns(categories);

            userThoughtsAndCrosses.CheckWord("Animal");
            userThoughtsAndCrosses.CheckWord("Bird");
            userThoughtsAndCrosses.CheckWord("Food");
            userThoughtsAndCrosses.CheckWord("Plane");
            userThoughtsAndCrosses.CheckWord("Fruit");

            userThoughtsAndCrosses.CreateGrid(categories);

            var finalScore = userThoughtsAndCrosses.Score;

            initialScore
                .Should()
                .Be(0);

            finalScore
                .Should()
                .Be(11);
        }
    }
}