using System.Collections.Concurrent;
using Chat.Pixenary;

namespace Chat.RoomManager
{
    public class Room
    {
        public ConcurrentDictionary<string, User> Users { get; }
        public GameThoughtsAndCrosses ThoughtsAndCrosses { get; private set; }
        public GameWordGame WordGame { get; private set; }
        public PixenaryManager PixenaryGame { get; private set; }


        public Room()
        {
            Users = new ConcurrentDictionary<string, User>();
        }

        public void AddUser(string name)
        {
            Users.TryAdd(name, new User(name));
        }

        public void SetThoughtsAndCrosses(GameThoughtsAndCrosses game)
        {
            ThoughtsAndCrosses = game;
        }

        public void SetWordGame(GameWordGame game)
        {
            WordGame = game;
        }

        public void SetPixenaryGame(PixenaryManager game)
        {
            PixenaryGame = game;
        }
    }
}