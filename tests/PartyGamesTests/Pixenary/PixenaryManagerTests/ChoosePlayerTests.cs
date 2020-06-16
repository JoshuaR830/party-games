using System;
using System.Collections.Generic;
using Chat.Letters;
using Chat.Pixenary;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Chat.WordGame.WordHelpers;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace PartyGamesTests.Pixenary.PixenaryManagerTests
{
    public class ChoosePlayerTests
    {
        private readonly PixenaryManager _pixenaryManager;
        private Dictionary<string, User> users;

        public ChoosePlayerTests()
        {
            var room = new Room();
            room.AddUser("Joshua");
            room.AddUser("Lydia");

            var roomName = Guid.NewGuid().ToString();
            Rooms.RoomsList.Add(roomName, room);

            var shuffleStringHelper = Substitute.For<IShuffleHelper<string>>();
            shuffleStringHelper
                .ShuffleList(Arg.Any<List<string>>())
                .Returns(new List<string> { "Joshua", "Lydia" });

            var shuffleWordHelper = Substitute.For<IShuffleHelper<WordData>>();
            shuffleWordHelper
                .ShuffleList(Arg.Any<List<WordData>>())
                .Returns(new List<WordData>());
            
            var wordCategoryHelper = Substitute.For<IWordCategoryHelper>();
            wordCategoryHelper.GetAllWordsWithCategories().Returns(new List<WordData>
            {
                new WordData { Word = "Sheep", Category = WordCategory.Animal },
                new WordData { Word = "Plane", Category = WordCategory.Vehicle },
                new WordData { Word = "Snowdrop", Category = WordCategory.Plant }
            });

            _pixenaryManager = new PixenaryManager(shuffleStringHelper, shuffleWordHelper, wordCategoryHelper, roomName);
        }

        [Fact]
        public void WhenAPlayerIsChosenThenAValueShouldBeSet()
        {
            _pixenaryManager.ChooseActivePlayer();
            _pixenaryManager.ActivePlayer.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void WhenAPlayerIsUpdatedADifferentPlayerShouldBeChosen()
        {
            _pixenaryManager.ChooseActivePlayer();
            var player1 = _pixenaryManager.ActivePlayer;
            _pixenaryManager.ChooseActivePlayer();
            var player2 = _pixenaryManager.ActivePlayer;
            player1.Should().NotBe(player2);
        }
    }
}