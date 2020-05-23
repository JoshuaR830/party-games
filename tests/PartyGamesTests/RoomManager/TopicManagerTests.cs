using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class TopicManagerTests
    {
        private TopicManager topicManager;

        public TopicManagerTests()
        {
            this.topicManager = new TopicManager();
        }
        
        [Fact]
        public void TheNumberOfInitialTopicsShouldBeGreaterThanOrEqualToNine()
        {
            topicManager
                .InitialTopics
                .Should()
                .HaveCountGreaterOrEqualTo(9);
        }
        
        [Fact]
        public void TheNumberOfChosenTopicsShouldBeGreaterThanNine()
        {
            topicManager.SetChosenTopics();
            
            topicManager
                .ChosenTopics
                .Should()
                .HaveCount(9);
        }
        
        [Fact]
        public void WhenMoreTopicsHaveBeenUsedThanTheInitialNumberOfTopicsStartAgain()
        {
            var topics1 = topicManager.InitialTopics;
            var iterations = topics1.Count / 9;


            for (var i = 0; i < iterations + 1; i++)
            {
                topicManager.SetChosenTopics();
            }

            var topics2 = topicManager.InitialTopics;

            topics1.Should().NotEqual(topics2);

            topicManager.NumTopicsUsed.Should().Be(9);
            
            topicManager
                .ChosenTopics
                .Should()
                .HaveCount(9);
        }
    }
}