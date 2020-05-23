using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class TopicManager
    {
        public List<string> InitialTopics { get; }
        public List<string> ChosenTopics { get; }
        public int NumTopicsUsed { get; }
        
        // ToDo: sort the topics into a random order at the beginning
        // ToDo: keep an index of how many you have used and just keep taking the next 9 from the list
        // ToDo: when at the end of the list, reset the NumTopicsUsed and shuffle the array again
        
        // ToDo: have a list shuffle helper - could be used for both the alphabet and this
    }
}