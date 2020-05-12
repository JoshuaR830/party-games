using System.Collections.Generic;

namespace Chat.Letters
{
    public class GamesLibrary
    {
        public List<Game> Games { get; }

        public GamesLibrary(List<Game> games)
        {
            Games = games;
        }
    }

    public class Game
    {
        public string GameName { get; }
        public string GameLink { get; }
        public string GameDescription { get; }

        public Game(string gameName, string gameLink, string gameDescription)
        {
            this.GameName = gameName;
            this.GameLink = gameLink;
            this.GameDescription = gameDescription;
        }
    }
}