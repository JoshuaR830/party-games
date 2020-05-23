using System;
using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class UserThoughtsAndCrosses : IUserThoughtsAndCrosses
    {
        public List<(string category, string userGuess, bool isAccepted)> WordsGrid { get; }
        public int Score { get; private set; }

        public UserThoughtsAndCrosses()
        {
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
            throw new NotImplementedException();
        }
    }
}