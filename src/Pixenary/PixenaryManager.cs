using System.Collections.Generic;

namespace Chat.Pixenary
{
    public class PixenaryManager : IPixenaryManager
    {
        public List<string> AllPlayers { get; private set; }
        public List<string> Grid { get; private set; }
        public string ActivePlayer { get; private set; }
        public string Word { get; private set; }

        public void CreateNewList(int gridSize)
        {
            throw new System.NotImplementedException();
        }

        public void AddPlayer(string name)
        {
            throw new System.NotImplementedException();
        }

        public void ChooseActivePlayer()
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePixel(int position)
        {
            throw new System.NotImplementedException();
        }

        public void ChooseWord()
        {
            throw new System.NotImplementedException();
        }

        public void ResetGame()
        {
            throw new System.NotImplementedException();
        }
    }
}