using System.Collections.Generic;
using System.Linq;
using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class ShuffleListHelperTests
    {
        [Theory]
        [InlineData(new object[] {new[] {"Me", "You", "Them", "Us"}})]
        [InlineData(new object[] {new[] {"O", "B", "N", "A"}})]
        [InlineData(new object[] {new[] {"O", "B"}})]
        [InlineData(new object[] {new[] {"B", "O"}})]
        [InlineData(new object[] {new[] {"A", "B"}})]
        [InlineData(new object[] {new[] {"B", "A"}})]
        [InlineData(new object[] {new[] {"A", "B", "A"}})]
        [InlineData(new object[] {new[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O"}})]
        public void WhenListToShuffleIsStringResponseShouldBeInDifferentOrderToInput(string[] data)
        {
            var shuffleHelper = new ShuffleHelper<string>();
            var list = new List<string>{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O"};
            var shuffledList = shuffleHelper.ShuffleList(list);
            shuffledList.Should().NotEqual(list);
        }
        
        [Theory]
        [InlineData(new[] {1, 2, 3, 4, 5, 6, 7})]
        [InlineData(new[] {9, 8, 7, 6 , 1})]
        [InlineData(new[] {1, 2, 1})]
        [InlineData(new[] {1, 2})]
        public void WhenListToShuffleIsIntResponseShouldBeInDifferentOrderToInput(int[] data)
        {
            var list = data.ToList();
            var shuffleHelper = new ShuffleHelper<int>();
            var shuffledList = shuffleHelper.ShuffleList(list);
            shuffledList.Should().NotEqual(list);
        }
    }
}