using System;
using System.Collections.Generic;
using Chat.RoomManager;
using Chat.WordGame.LocalDictionaryHelpers;
using Newtonsoft.Json;

namespace Chat.Pixenary
{
    public class PixenaryManager : IPixenaryManager
    {
        private readonly IShuffleHelper<string> _shuffleStringHelper;
        private readonly IWordCategoryHelper _wordCategoryHelper;
        private readonly IShuffleHelper<WordData> _shuffleWordDataHelper;
        public List<string> Players { get; private set; }
        public List<string> Grid { get; private set; }
        public List<WordData> WordsWithCategories { get; private set; }
        public string ActivePlayer { get; private set; }
        public WordData Word { get; private set; }
        public int PlayerTurns { get; private set; }
        public int WordsUsed { get; private set; }

        public PixenaryManager(IShuffleHelper<string> shuffleStringHelper, IShuffleHelper<WordData> shuffleWordDataHelper, IWordCategoryHelper wordCategoryHelper, string roomId)
        {
            _shuffleStringHelper = shuffleStringHelper;
            _wordCategoryHelper = wordCategoryHelper;
            _shuffleWordDataHelper = shuffleWordDataHelper;
            Players = new List<string>();
            Grid = new List<string>();
            
            WordsWithCategories = _wordCategoryHelper.GetAllWordsWithCategories();
            WordsWithCategories = _shuffleWordDataHelper.ShuffleList(WordsWithCategories);
            
            var users = Rooms.RoomsList[roomId].Users;
            foreach(var player in users)
                Players.Add(player.Key);

            if(Players.Count > 1)
                Players = _shuffleStringHelper.ShuffleList(Players);
        }

        public void AddPlayersToGame(IEnumerable<string> users)
        {
            Players.Clear();
            foreach (var user in users)
            {
                Players.Add(user);
            }

            Players = _shuffleStringHelper.ShuffleList(Players);
        }

        public void CreateNewList(int gridSize)
        {
            Grid = new List<string>();
            for(var x = 0; x < gridSize * gridSize; x++)
                Grid.Add(null);
        }

        public void ChooseActivePlayer()
        {
            if (PlayerTurns == Players.Count)
            {
                if(Players.Count > 1)
                    Players = _shuffleStringHelper.ShuffleList(Players);
                PlayerTurns = 0;
            }

            ActivePlayer = Players[PlayerTurns];
            Console.WriteLine($">>> {JsonConvert.SerializeObject(Players)}");
            PlayerTurns++;
        }

        public void UpdatePixel(int position, string colour)
        {
            Grid[position] = colour;
        }

        // ToDo: allow selection by category
        public void ChooseWord()
        {
            if (WordsUsed == WordsWithCategories.Count)
            {
                WordsWithCategories = _shuffleWordDataHelper.ShuffleList(WordsWithCategories);
                WordsUsed = 0;
            }
            Word = WordsWithCategories[WordsUsed];
            WordsUsed ++;
        }

        public void ResetGame()
        {
            var count = Grid.Count;
            CreateNewList(count);
            Word = null;
            ActivePlayer = null;
        }
    }
}