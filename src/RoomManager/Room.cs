using System.Collections.Concurrent;
using Chat.Balderdash;
using Chat.Pixenary;

namespace Chat.RoomManager
{
    public class Room
    {
        public ConcurrentDictionary<string, User> Users { get; }
        public GameThoughtsAndCrosses ThoughtsAndCrosses { get; private set; }
        public GameWordGame Word { get; private set; }
        public PixenaryManager Pixenary { get; private set; }

        public BalderdashManager Balderdash { get; private set; }

        public Room()
        {
            Users = new ConcurrentDictionary<string, User>();
        }

        public void AddUser(string name)
        {
            Users.TryAdd(name, new User(name));
        }

        public void SetThoughtsAndCrossesGame(GameThoughtsAndCrosses game)
        {
            ThoughtsAndCrosses = game;
        }

        public void SetWordGame(GameWordGame game)
        {
            Word = game;
        }

        public void SetPixenaryGame(PixenaryManager game)
        {
            Pixenary = game;
        }

        public void SetBalderdashGame(BalderdashManager game)
        {
            Balderdash = game;
        }
    }
}