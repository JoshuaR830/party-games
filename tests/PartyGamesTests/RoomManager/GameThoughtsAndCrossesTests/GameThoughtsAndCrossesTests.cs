using System.Collections.Generic;
using Chat.RoomManager;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.RoomManager.GameThoughtsAndCrossesTests
{
    public class GameThoughtsAndCrossesTests
    {
        private readonly IShuffleHelper<string> _shuffleHelper;

        public GameThoughtsAndCrossesTests()
        {
            _shuffleHelper = Substitute.For<IShuffleHelper<string>>();
        }
        
        [Fact]
        public void WhenLetterSetIsCalledLetterShouldHaveAValue()
        {
            _shuffleHelper
                .ShuffleList(Arg.Any<List<string>>())
                .Returns(new List<string>{ "B", "A", "D", "C", "XYZ", "W", "V", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "G", "F", "E" });
            
            var thoughtsAndCrosses = new GameThoughtsAndCrosses(_shuffleHelper);

            thoughtsAndCrosses.SetLetter();
            var letter1 = thoughtsAndCrosses.Letter.Letter;
            thoughtsAndCrosses.SetLetter();
            var letter2 = thoughtsAndCrosses.Letter.Letter;

            thoughtsAndCrosses
                .Letter
                .Letter
                .Should()
                .NotBeNullOrWhiteSpace();
            
            letter1.Should().NotBeEquivalentTo(letter2);
        }
        
        [Fact]
        public void SetTopics()
        {
            _shuffleHelper
                .ShuffleList(Arg.Any<List<string>>())
                .Returns(new List<string>{"Girls name", "Boys name", "Book", "Fictional character", "Company / Brand", "Something outside", "Hobby", "Toy", "Electrical item", "Kitchen item", "Body part", "Colour", "Song", "Something savoury", "Something sweet", "Colour", "Toy", "Movie", "Job / Occupation", "Sport / Game", "Place", "Food", "TV programme", "Transport", "Pet", "Actor / Actress", "Family member", "Holiday destination", "Weather", "Animal / Bird", "Something you make", "Fundraising Activity", "Drink", "Ice cream", "Artist", "Musical instrument", "Fundraising Activity"});
            
            var thoughtsAndCrosses = new GameThoughtsAndCrosses(_shuffleHelper);

            thoughtsAndCrosses.CalculateTopics();
            var topicsSet1 = thoughtsAndCrosses.Topics.ChosenTopics;
            thoughtsAndCrosses.CalculateTopics();
            var topicsSet2 = thoughtsAndCrosses.Topics.ChosenTopics;
            
            thoughtsAndCrosses
                .Topics
                .ChosenTopics
                .Should()
                .HaveCount(9);

            topicsSet1.Should().NotEqual(topicsSet2);
        }
    }
}