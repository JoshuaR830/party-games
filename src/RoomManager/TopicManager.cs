using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class TopicManager
    {
        private readonly IShuffleHelper<string> _shuffleHelper;
        
        public List<string> InitialTopics { get; private set; }
        public List<string> ChosenTopics { get; private set; }
        public int NumTopicsUsed { get; private set; }

        public TopicManager(IShuffleHelper<string> shuffleHelper)
        {
            _shuffleHelper = shuffleHelper;
            InitialTopics = new List<string>{"Boys name", "Girls name", "Hobby", "Fictional character", "Something outside", "Book", "Electrical item", "Kitchen item", "Body part", "Song", "Something savoury", "Something sweet", "Colour", "Toy", "Movie", "Job / Occupation", "Sport / Game", "Place", "Food", "TV programme", "Transport", "Pet", "Actor / Actress", "Family member", "Holiday destination", "Weather", "Animal / Bird", "Something you make", "Drink", "Ice cream", "Artist", "Company / Brand", "Musical instrument", "Fundraising Activity"};
            InitialTopics = _shuffleHelper.ShuffleList(InitialTopics);
        }

        public void SetChosenTopics()
        {
            if (NumTopicsUsed + 9 > InitialTopics.Count)
            {
                InitialTopics = _shuffleHelper.ShuffleList(InitialTopics);
                NumTopicsUsed = 0;
            }
            
            var nineTopics = new List<string>();
            for (var i = 0; i < 9; i++)
            {
                nineTopics.Add(InitialTopics[NumTopicsUsed+i]);
            }

            ChosenTopics = nineTopics;
            NumTopicsUsed += 9;
        }
    }
}