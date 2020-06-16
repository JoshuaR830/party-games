using System;
using System.Collections.Generic;
using Chat.Pixenary;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using PartyGamesTests.Pixenary.WordCategoryHelperTests;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class ResetGameTests
    {
        private readonly PixenaryManager _pixenaryManager;

        public ResetGameTests()
        {
            var room = new Room();
            room.AddUser("Joshua");
            room.AddUser("Lydia");

            var roomName = Guid.NewGuid().ToString();
            Rooms.RoomsList.TryAdd(roomName, room);

            
            var wordList = new List<WordData>
            {
                new WordData { Word = "Sheep", Category = WordCategory.Animal },
                new WordData { Word = "Plane", Category = WordCategory.Vehicle },
                new WordData { Word = "Snowdrop", Category = WordCategory.Plant }
            }; 
            
            var shuffleStringHelper = Substitute.For<IShuffleHelper<string>>();
            shuffleStringHelper
                .ShuffleList(Arg.Any<List<string>>())
                .Returns(new List<string> { "Joshua", "Lydia" });

            var shuffleWordHelper = Substitute.For<IShuffleHelper<WordData>>();
            shuffleWordHelper
                .ShuffleList(Arg.Any<List<WordData>>())
                .Returns(wordList);
            
            var wordCategoryHelper = Substitute.For<IWordCategoryHelper>();
            wordCategoryHelper.GetAllWordsWithCategories().Returns(wordList);

            _pixenaryManager = new PixenaryManager(shuffleStringHelper, shuffleWordHelper, wordCategoryHelper, roomName);
            
            _pixenaryManager.ChooseActivePlayer();
            _pixenaryManager.ChooseWord();
            _pixenaryManager.CreateNewList(10);
            _pixenaryManager.UpdatePixel(10, "#ff0000");
        }

        [Fact]
        public void WhenGameIsResetPlayerShouldBeReset()
        {
            _pixenaryManager.ActivePlayer.Should().NotBeNull();
            _pixenaryManager.ResetGame();
            _pixenaryManager.ActivePlayer.Should().BeNull();
        }

        [Fact]
        public void WhenGameIsResetWordShouldBeReset()
        {
            _pixenaryManager.Word.Should().NotBeNull();
            _pixenaryManager.ResetGame();
            _pixenaryManager.Word.Should().BeNull();
        }

        [Fact]
        public void WhenGameIsResetListShouldBeReset()
        {
            _pixenaryManager.Grid.Should().Contain(x => x == "#ff0000");
            _pixenaryManager.ResetGame();
            _pixenaryManager.Grid.Should().OnlyContain(x => x == null);
        }
    }
}