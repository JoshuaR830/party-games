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
    public class CreateNewListTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public CreateNewListTests()
        {
            var roomName = Guid.NewGuid().ToString();
            Rooms.RoomsList.Add(roomName, new Room());

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
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(10, 100)]
        [InlineData(20, 400)]
        [InlineData(100, 10000)]
        public void WhenTheListIsCreatedThereShouldBeTehCorrectNumberOfElements(int gridSize, int expectedCellNumber)
        {
            _pixenaryManager.CreateNewList(gridSize);
            var actualCellNumber = _pixenaryManager.Grid.Count;
            actualCellNumber.Should().Be(expectedCellNumber);
        }

        [Fact]
        public void WhenTheListIsCreatedAllElementsShouldBeNull()
        {
            _pixenaryManager.CreateNewList(10);
            var cells = _pixenaryManager.Grid;

            cells.Should().OnlyContain(x => x == null);
        }
    }
}