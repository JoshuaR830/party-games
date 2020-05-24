using System;
using System.Collections.Generic;
using System.Linq;

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
            // ToDo: find the position in the list of the category
            // ToDo: add user guess to the WordsGrid
            throw new NotImplementedException();
        }

        public void CheckWord(string category)
        {
            // ToDo: set the status to checked
            throw new NotImplementedException();
        }

        public void UncheckWord(string category)
        {
            // ToDo: set the status to unchecked
            throw new NotImplementedException();
        }

        public void CalculateScore()
        {
            var isAcceptedList = WordsGrid.Select(x => x.isAccepted).ToList();
            Score = _scoreHelper.CalculateThoughtsAndCrossesScore(isAcceptedList);
        }
    }
}