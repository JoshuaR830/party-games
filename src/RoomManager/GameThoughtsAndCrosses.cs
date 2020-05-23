using System;
using Chat.Pages;

namespace Chat.RoomManager
{
    public class GameThoughtsAndCrosses
    {
        public LetterManager Letter { get; }
        public TopicManager Topics { get; }
        public int TimerSeconds { get; }

        public GameThoughtsAndCrosses()
        {
            
        }

        public void SetLetter()
        {
            // ToDo: Ask the letter helper for a letter
            // ToDo: Letter helper will have a list of currently guessed letters
            // ToDo: this should be an instance of an object as it will need to manage state for the system
            // ToDo: Reset the currently guessed letters when length of the list is length of alphabet list
            throw new NotImplementedException();
        }

        public void CalculateTopics()
        {
            // ToDo: Create a topics helper that will return a list of chosen topics
            // ToDo: The topic helper will also have a list of initial topics - this will be what it pulls the topics from
            // ToDo: The topics helper should have a list of known topics - for that reason it needs to have state
            throw new NotImplementedException();
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