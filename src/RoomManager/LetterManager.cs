using System;
using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class LetterManager
    {
        private readonly IShuffleHelper<string> _shuffleHelper;

        public List<string> Alphabet { get; }
        public int NumLettersUsed { get; private set; }
        public string Letter { get; private set; }
        
        public LetterManager(IShuffleHelper<string> shuffleHelper)
        {
            _shuffleHelper = shuffleHelper;
        }

        public void SetLetter()
        {
            throw new NotImplementedException();
        }

        // ToDo: sort the letters into a random order at the beginning
        // ToDo: keep an index of how many you have used and just keep taking the next 1 from the list
        // ToDo: when at the end of the list, reset the NumTopicsUsed and shuffle the array again
        
        // ToDo: have a list shuffle helper - could be used for both the alphabet and this
    }
}