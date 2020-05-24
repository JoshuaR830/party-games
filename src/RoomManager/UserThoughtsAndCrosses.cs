using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.RoomManager
{
    public class UserThoughtsAndCrosses : IUserThoughtsAndCrosses
    {
        private IScoreHelper _scoreHelper;
        public List<(string category, string userGuess, bool isAccepted)> WordsGrid { get; }
        public int Score { get; private set; }

        public UserThoughtsAndCrosses(IScoreHelper scoreHelper)
        {
            _scoreHelper = scoreHelper;
        }

        public void CreateGrid(List<string> categories)
        {
            // ToDo: Put the categories into a grid in a random order - categories.[OrderBy (a => rand.Next())]
            // ToDo return the grid to the user
            throw new NotImplementedException();
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