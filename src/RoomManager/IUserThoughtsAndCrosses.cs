using System.Collections.Generic;

namespace Chat.RoomManager
{
    public interface IUserThoughtsAndCrosses
    {
        void CreateGrid(List<string> categories);
        void ManageGuess(string category, string userGuess);
        void CheckWord(string category);
        void UncheckWord(string category);
        void CalculateScore(List<bool> scores);
    }
}