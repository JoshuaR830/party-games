using System;

namespace Chat.RoomManager
{
    public class GameThoughtsAndCrosses : IGameThoughtsAndCrosses
    {
        private readonly IShuffleHelper<string> _shuffleHelper;
        public LetterManager Letter { get; }
        public TopicManager Topics { get; }
        public int TimerSeconds { get; }

        public GameThoughtsAndCrosses(IShuffleHelper<string> shuffleHelper)
        {
            _shuffleHelper = shuffleHelper;
            Letter = new LetterManager(_shuffleHelper);
            Topics = new TopicManager(_shuffleHelper);
        }

        public void SetLetter()
        {
            Letter.SetLetter();
        }

        public void CalculateTopics()
        {
            Topics.SetChosenTopics();
        }

        public void SetTimer(int minutes, int seconds)
        {
            // ToDo: Multiply minutes by 60
            // ToDo: add minutes to seconds
            // ToDo: save
            throw new NotImplementedException();
        }
    }
}