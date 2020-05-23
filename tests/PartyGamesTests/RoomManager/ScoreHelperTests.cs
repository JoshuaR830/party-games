using System.Collections.Generic;
using System.Linq;
using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class ScoreHelperTests
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                // Everything
                new object[] { new[] {true, true, true, true, true, true, true, true, true}, 33},
                new object[] { new[] {true, true, true, true, false, true, true, true, true}, 20},

                // No lines
                new object[] { new[] {false, false, false, false, false, false, false, false, false}, 0},
                new object[] { new[] {true, false, false, false, false, false, false, false, false}, 1},
                new object[] { new[] {false, true, false, false, false, false, false, false, false}, 1},
                new object[] { new[] {false, false, true, false, false, false, false, false, false}, 1},
                new object[] { new[] {false, false, false, true, false, false, false, false, false}, 1},
                new object[] { new[] {false, false, false, false, true, false, false, false, false}, 1},
                new object[] { new[] {false, false, false, false, false, true, false, false, false}, 1},
                new object[] { new[] {false, false, false, false, false, false, true, false, false}, 1},
                new object[] { new[] {false, false, false, false, false, false, false, true, false}, 1},
                new object[] { new[] {false, false, false, false, false, false, false, false, true}, 1},

                // Horizontal
                new object[] { new[] {true, true, true, false, false, false, false, false, false}, 6},
                new object[] { new[] {false, false, false, true, true, true, false, false, false}, 6},
                new object[] { new[] {false, false, false, false, false, false, true, true, true}, 6},
                
                // Vertical
                new object[] { new[] {true, false, false, true, false, false, true, false, false}, 6},
                new object[] { new[] {false, true, false, false, true, false, false, true, false}, 6},
                new object[] { new[] {false, false, true, false, false, true, false, false, true}, 6},
                
                // Diagonals
                new object[] { new[] {true, false, false, false, true, false, false, false, true}, 6},
                new object[] { new[] {false, false, true, false, true, false, true, false, false}, 6},
                new object[] { new[] {true, false, true, false, true, false, true, false, true}, 11}
            };


        [Theory]
        [MemberData(nameof(Data))]
        public void WhenGivenAListOfBoolAScoreShouldBeCalculated(bool[] scoreList, int expectedScore)
        {
            var scoreHelper = new ScoreHelper();
            var score = scoreHelper.CalculateThoughtsAndCrossesScore(scoreList.ToList());
            
            score.Should().Be(expectedScore);
        }
        
    }
}