using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class LetterManager
    {
        private readonly IShuffleHelper<string> _shuffleHelper;

        public List<string> Alphabet { get; private set; }
        public int NumLettersUsed { get; private set; }
        public string Letter { get; private set; }
        
        public LetterManager(IShuffleHelper<string> shuffleHelper)
        {
            _shuffleHelper = shuffleHelper;
            Alphabet = new List<string>{"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "XYZ"};
            Alphabet = _shuffleHelper.ShuffleList(Alphabet);
        }

        public void SetLetter()
        {
            if (Alphabet.Count <= NumLettersUsed)
            {
                NumLettersUsed = 0;
                Alphabet = _shuffleHelper.ShuffleList(Alphabet);
            }
            
            Letter = Alphabet[NumLettersUsed];
            NumLettersUsed ++;
        }
    }
}