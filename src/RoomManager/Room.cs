using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class Room
    {
        public Dictionary<string, User> Users { get; }
        public GameThoughtsAndCrosses GameThoughtsAndCrosses { get; private set; }


        public Room()
        {
            Users = new Dictionary<string, User>();
        }

        public void AddUser(string name)
        {
            Users.Add(name, new User(name));
        }

        public void SetThoughtsAndCrosses(GameThoughtsAndCrosses game)
        {
            GameThoughtsAndCrosses = game;
        }
    }
}