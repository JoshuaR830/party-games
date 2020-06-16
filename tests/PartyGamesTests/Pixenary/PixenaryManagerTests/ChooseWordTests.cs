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
    public class ChooseWordTests
    {
        private readonly PixenaryManager _pixenaryManager;
        private readonly List<WordData> _wordList;

        public ChooseWordTests()
        {
            var roomName = Guid.NewGuid().ToString();
            Rooms.RoomsList.Add(roomName, new Room());

            _wordList = new List<WordData>
            {
                new WordData { Word = "Sheep", Category = WordCategory.Animal },
                new WordData { Word = "Plane", Category = WordCategory.Vehicle },
                new WordData { Word = "Snowdrop", Category = WordCategory.Plant }
            }; 
            
            var shuffleStringHelper = Substitute.For<IShuffleHelper<string>>();
            var shuffleWordHelper = Substitute.For<IShuffleHelper<WordData>>();
            shuffleWordHelper
                .ShuffleList(Arg.Any<List<WordData>>())
                .Returns(_wordList);
            
            var wordCategoryHelper = Substitute.For<IWordCategoryHelper>();
            wordCategoryHelper.GetAllWordsWithCategories().Returns(_wordList);

            _pixenaryManager = new PixenaryManager(shuffleStringHelper, shuffleWordHelper, wordCategoryHelper, roomName);
        }
        
        [Fact]
        public void WhenChooseWordIsCalledANewWordShouldBeSet()
        {
            _pixenaryManager.ChooseWord();
            _wordList.Should().ContainEquivalentOf(_pixenaryManager.Word);
            _pixenaryManager.Word.Should().BeEquivalentTo(_wordList[0]);
        }

        [Fact]
        public void WhenAWordIsAlreadySetAndChooseWordIsCalledThenANewWordShouldBeChosen()
        {
            _pixenaryManager.ChooseWord();
            var originalWord = _pixenaryManager.Word;
            _pixenaryManager.ChooseWord();
            var newWord = _pixenaryManager.Word;

            originalWord.Should().NotBe(newWord);
        }
        
        [Fact]
        public void WhenMultipleWordsAreRequestedTheyShouldBeReturned()
        {
            _pixenaryManager.ChooseWord();
            _pixenaryManager.Word.Should().BeEquivalentTo(_wordList[0]);
            _pixenaryManager.ChooseWord();
            _pixenaryManager.Word.Should().BeEquivalentTo(_wordList[1]);
            _pixenaryManager.ChooseWord();
            _pixenaryManager.Word.Should().BeEquivalentTo(_wordList[2]);
            _pixenaryManager.ChooseWord();
            _pixenaryManager.Word.Should().BeEquivalentTo(_wordList[0]);
        }
    }
}