using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class LetterManager
    {
        private List<string> Letters { get; }
        public int NumLettersUsed { get; private set; }
        public string Letter { get; private set; }
        
        // ToDo: sort the letters into a random order at the beginning
        // ToDo: keep an index of how many you have used and just keep taking the next 1 from the list
        // ToDo: when at the end of the list, reset the NumTopicsUsed and shuffle the array again
        
        // ToDo: have a list shuffle helper - could be used for both the alphabet and this
    }
}