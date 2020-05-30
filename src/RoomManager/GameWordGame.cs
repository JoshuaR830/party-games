using System.Collections.Generic;
using Chat.Letters;

namespace Chat.RoomManager
{
    public class GameWordGame : IGameWordGame
    {
        public List<LetterScore> Letters;
        
        public void GetLetters()
        {
            var lettersHelper = new LettersHelper();
            lettersHelper.GetLetters();
            
            Letters = lettersHelper.Letters;
        }

        public void SetTimer(int minutes, int seconds)
        {
            throw new System.NotImplementedException();
        }
    }
}