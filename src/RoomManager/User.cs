using System.Collections.Generic;
using Chat.Balderdash;
using Chat.Pixenary;

namespace Chat.RoomManager
{
    public class User
    {
        public string Name { get; }
        public int Score { get; private set; }

        public UserThoughtsAndCrossesGame ThoughtsAndCrossesGame { get; private set; }
        public UserWordGame WordGame { get; private set; }
        public UserPixenaryGame PixenaryGame { get; private set; }
        public UserBalderdashGame BalderdashGame { get; private set; }

        public User(string name)
        {
            Name = name;
            Score = 0;
        }

        public void SetScore(int score)
        {
            Score = score;
        }

        public void SetUpGame<T>(T game)
        {
            var type = game.GetType().Name;
            type = type.Replace("User", "");
            var propertyInfo = GetType().GetProperty(type);
            propertyInfo?.SetValue(this, game);
        }
    }
}