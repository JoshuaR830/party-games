using System.Collections.Generic;
using Chat.RoomManager;
using Chat.WordGame.WordHelpers;

namespace Chat.Pixenary
{
    public interface IPixenaryManager
    {
        void CreateNewList(int gridSize);
        void ChooseActivePlayer();
        void ChooseWord();
        void ResetGame();
        void UpdatePixel(int position, string colour);
    }
}