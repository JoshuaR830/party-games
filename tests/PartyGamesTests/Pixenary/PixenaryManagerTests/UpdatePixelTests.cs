using System;
using System.Collections.Generic;
using Chat.Pixenary;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class UpdatePixelTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public UpdatePixelTests()
        {
            var roomName = Guid.NewGuid().ToString();
            Rooms.RoomsList.Add(roomName, new Room());
            var shuffleHelper = Substitute.For<IShuffleHelper<string>>();
            
            
            var wordList = new List<WordData>
            {
                new WordData { Word = "Sheep", Category = WordCategory.Animal },
                new WordData { Word = "Plane", Category = WordCategory.Vehicle },
                new WordData { Word = "Snowdrop", Category = WordCategory.Plant }
            }; 
            
            var shuffleStringHelper = Substitute.For<IShuffleHelper<string>>();
            var shuffleWordHelper = Substitute.For<IShuffleHelper<WordData>>();
            shuffleWordHelper
                .ShuffleList(Arg.Any<List<WordData>>())
                .Returns(wordList);
            
            var wordCategoryHelper = Substitute.For<IWordCategoryHelper>();
            wordCategoryHelper.GetAllWordsWithCategories().Returns(wordList);

            _pixenaryManager = new PixenaryManager(shuffleStringHelper, shuffleWordHelper, wordCategoryHelper, roomName);

            _pixenaryManager.CreateNewList(10);
        }

        [Theory]
        [InlineData(0, "#ff00ff")]
        [InlineData(0, "#ffffff")]
        [InlineData(1, "#ffaaff")]
        [InlineData(1, "#ff11ff")]
        [InlineData(10, "#ffffff")]
        [InlineData(10, "#ff11ff")]
        [InlineData(11, "#ff00ff")]
        [InlineData(11, "#0000ff")]
        [InlineData(99, "#1100ff")]
        [InlineData(99, "#4400ff")]
        public void WhenAPixelLocationIsSetThenTheListShouldBeUpdated(int pixel, string colour)
        {
            _pixenaryManager.UpdatePixel(pixel, colour);
            _pixenaryManager.Grid[pixel].Should().Be(colour);
        }
    }
}