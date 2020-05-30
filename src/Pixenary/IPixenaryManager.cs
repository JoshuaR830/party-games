namespace Chat.Pixenary
{
    public interface IPixenaryManager
    {
        void CreateNewList(int gridSize);
        void AddPlayer(string name);
        void ChooseActivePlayer();
        void ChooseWord();
        void ResetGame();
        void UpdatePixel(int position);
    }
}