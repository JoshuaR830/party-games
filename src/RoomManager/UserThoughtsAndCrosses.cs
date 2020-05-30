using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Chat.RoomManager
{
    public class UserThoughtsAndCrosses : IUserThoughtsAndCrosses
    {
        private readonly IScoreHelper _scoreHelper;
        private IShuffleHelper<string> _shuffleHelper;
        public List<(string category, string userGuess, bool isAccepted)> WordsGrid { get; private set; }
        public int Score { get; private set; }

        public UserThoughtsAndCrosses(IScoreHelper scoreHelper, IShuffleHelper<string> shuffleHelper)
        {
            _scoreHelper = scoreHelper;
            _shuffleHelper = shuffleHelper;
            WordsGrid = new List<(string category, string userGuess, bool isAccepted)>();
        }

        public void CreateGrid(List<string> categories)
        {
            var shuffledCategories = _shuffleHelper.ShuffleList(categories);
            WordsGrid = new List<(string category, string userGuess, bool isAccepted)>();

            foreach (var category in shuffledCategories)
            {
                WordsGrid.Add((category, "", false));
            }
        }

        public void ManageGuess(string category, string userGuess)
        {
            var index = GetIndex(category);

            if (index < 0)
                return;

            var valueTuple = WordsGrid[index];
            WordsGrid[index] = (valueTuple.category, userGuess, valueTuple.isAccepted);
        }

        public void CheckWord(string category)
        {
            var index = GetIndex(category);
            
            if (index < 0)
                return;
            
            var valueTuple = WordsGrid[index];
            WordsGrid[index] = (valueTuple.category, valueTuple.userGuess, true);
        }

        public void UncheckWord(string category)
        {
            var index = GetIndex(category);

            if (index < 0)
                return;
            
            var valueTuple = WordsGrid[index];
            WordsGrid[index] = (valueTuple.category, valueTuple.userGuess, false);
        }

        public void CalculateScore()
        {
            var isAcceptedList = WordsGrid.Select(x => x.isAccepted).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(isAcceptedList));
            Score = _scoreHelper.CalculateThoughtsAndCrossesScore(isAcceptedList);
            Console.WriteLine(Score);
        }

        private int GetIndex(string category)
        {
            var cells = WordsGrid.Where(x => x.category == category).ToList();

            if (!cells.Any())
                return -1;
            
            return WordsGrid.IndexOf(cells.First());
        }
    }
}