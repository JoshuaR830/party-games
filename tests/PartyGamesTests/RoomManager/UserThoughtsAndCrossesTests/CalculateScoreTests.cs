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
            var scoreHelper = new ScoreHelper();

            var categories = new List<string> {"Animal", "Colour", "Bird", "News", "Food", "Boat", "Plane", "Car", "Fruit"};
            
            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            _shuffleHelper
                .ShuffleList(categories)
                .Returns(categories);
            
            var userThoughtsAndCrosses = new UserThoughtsAndCrossesGame(scoreHelper, _shuffleHelper);
            userThoughtsAndCrosses.CreateGrid(categories);
            userThoughtsAndCrosses.CalculateScore();
            var initialScore = userThoughtsAndCrosses.Score;

            userThoughtsAndCrosses.CheckWord("Animal");
            userThoughtsAndCrosses.CheckWord("Bird");
            userThoughtsAndCrosses.CheckWord("Food");
            userThoughtsAndCrosses.CheckWord("Plane");
            userThoughtsAndCrosses.CheckWord("Fruit");

            userThoughtsAndCrosses.CalculateScore();
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