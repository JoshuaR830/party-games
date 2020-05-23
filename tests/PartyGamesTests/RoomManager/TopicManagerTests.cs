using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class TopicManagerTests
    {
        private readonly TopicManager _topicManager;

        public TopicManagerTests()
        {
            var shuffleHelper = new ShuffleHelper<string>();
            _topicManager = new TopicManager(shuffleHelper);
        }
        
        [Fact]
        public void TheNumberOfInitialTopicsShouldBeGreaterThanOrEqualToNine()
        {
            _topicManager
                .InitialTopics
                .Should()
                .HaveCountGreaterOrEqualTo(9);
        }
        
        [Fact]
        public void TheNumberOfChosenTopicsShouldBeNine()
        {
            _topicManager.SetChosenTopics();
            
            _topicManager
                .ChosenTopics
                .Should()
                .HaveCount(9);
        }
        
        [Fact]
        public void WhenMoreTopicsHaveBeenUsedThanTheInitialNumberOfTopicsStartAgain()
        {
            var topics1 = _topicManager.InitialTopics;
            var iterations = topics1.Count / 9;


            for (var i = 0; i < iterations + 1; i++)
            {
                _topicManager.SetChosenTopics();
            }

            var topics2 = _topicManager.InitialTopics;

            topics1.Should().NotEqual(topics2);

            _topicManager.NumTopicsUsed.Should().Be(9);
            
            _topicManager
                .ChosenTopics
                .Should()
                .HaveCount(9);
        }
    }
}