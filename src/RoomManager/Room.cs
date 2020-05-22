using System.Collections.Generic;
using System.Linq;
using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.RoomManager
{
    public class Room
    {
        public Dictionary<string, User> Users { get; }

        public Room()
        {
            Users = new Dictionary<string, User>();
        }

        public void AddUser(string name)
        {
            Users.Add(name, new User(name));
        }
    }
}