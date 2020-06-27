using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chat.Balderdash;
using Chat.GameManager;
using Chat.RoomManager;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Chat.Hubs
{
    public class BalderdashHub : Hub
    {
        
        private readonly IGameManager _gameManager;
        private readonly IShuffleHelper<GuessMade> _shuffleGameOrder;
        private readonly IBalderdashScoreCalculator _balderdashScoreCalculator;

        public BalderdashHub(IGameManager gameManager, IShuffleHelper<GuessMade> shuffleGameOrder, IBalderdashScoreCalculator balderdashScoreCalculator)
        {
            _gameManager = gameManager;
            _shuffleGameOrder = shuffleGameOrder;
            _balderdashScoreCalculator = balderdashScoreCalculator;
        }

        public async Task StartUp(string roomId, string name, int gameId)
        {
            Console.WriteLine("Hi");
            Console.WriteLine(roomId);
            Console.WriteLine(name);
            // ToDo: this is the wrong game type
            if (!Rooms.RoomsList.ContainsKey(roomId))
            {
                Console.WriteLine("New room 2");
                _gameManager.SetupNewGame(roomId, name, GameType.Balderdash);
                // Rooms.RoomsList[roomId].Balderdash;
                // Console.WriteLine(name);
            }

            Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList));
            Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList[roomId].Users));
            
            // Todo: This is the wrong game type  
            if (!Rooms.RoomsList[roomId].Users.ContainsKey(name))
            {
                var game = Rooms.RoomsList[roomId].Balderdash;
                _gameManager.SetUpNewBalderdashUser(roomId, name, game);
                Rooms.RoomsList[roomId].Balderdash.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Select(x => x.Key));
                Console.WriteLine(name);
            }
            
            // ToDo: add the game to the server and set up anything that needs to be set
            // ToDo: Trigger off the back of login
        }

        public async Task SetUpUser()
        {
            // ToDo: connect user name to server
            // ToDO: connect user to group server for group updates
            
            // ToDo: add user to the game list
        }

        public async Task BalderdashScores(string roomId, string playerWhoGuessed, string playerWhoProposed)
        {
            // ToDo: need to take who is giving answer and who they chose
            // ToDo: server needs to work out the scores
            _balderdashScoreCalculator.CalculateGuesser(roomId, playerWhoGuessed, playerWhoProposed);
            _balderdashScoreCalculator.CalculateProposer(roomId, playerWhoGuessed, playerWhoProposed);

            var users = Rooms.RoomsList[roomId].Users;
            // users[playerWhoGuessed].BalderdashGame.SetScore(guesserScore);
            // users[playerWhoProposed].BalderdashGame.SetScore(proposerScore);
        }

        public async Task BalderdashDiscardSimilar()
        {
            // ToDo: if number discarded = 2 start next round
            // ToDo: call reset game
            
            // Todo: have a property - discarded this round
            // ToDo: update score if user gets theirs discarded
        }

        public async Task ResetGame()
        {
            // ToDo: keep scores, keep player order but update dasher and clear screens
            // ToDo: reset teh discarded number
        }

        public async Task PassToDasher(string roomId, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, name);

            
            // ToDo: remove fake setup
            FakeSetup(roomId);
            
            
            var guesses = new List<GuessMade>();
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                guesses.Add(new GuessMade(user.Value.BalderdashGame.Guess, user.Key));
            }

            guesses = _shuffleGameOrder.ShuffleList(guesses);

            await Clients.Group(name).SendAsync("ReceivedGuesses", guesses);
            
            

            // ToDo: send text and who
            // ToDo: server knows who dasher is so can send it easily
            // ToDo: keep a record of all of a rounds guesses - send all back to the dasher in random order
            // ToDo: object would look like {"User": "Name", "Data": "This is my guess"}
            // ToDo: When user presses submit it will send to the server
            // ToDo: Keep track of the number of guesses in server 
        }

        private void FakeSetup(string roomId)
        {
            Rooms.RoomsList[roomId].Users.Clear();
            Rooms.RoomsList[roomId].AddUser("Joshua");
            Rooms.RoomsList[roomId].Users["Joshua"].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList[roomId].Users["Joshua"].BalderdashGame.SetGuess("Joshua's guess");
            
            Rooms.RoomsList[roomId].AddUser("Lydia");
            Rooms.RoomsList[roomId].Users["Lydia"].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList[roomId].Users["Lydia"].BalderdashGame.SetGuess("Lydia's guess");
            
            Rooms.RoomsList[roomId].AddUser("Andrew");
            Rooms.RoomsList[roomId].Users["Andrew"].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList[roomId].Users["Andrew"].BalderdashGame.SetGuess("Andrew's guess");

            Rooms.RoomsList[roomId].AddUser("Kerry");
            Rooms.RoomsList[roomId].Users["Kerry"].SetUpGame(new UserBalderdashGame());
            Rooms.RoomsList[roomId].Users["Kerry"].BalderdashGame.SetGuess("Kerry's guess");
        }

        public class GuessMade
        {
            public string Name { get; }
            public string Guess { get; }

            public GuessMade(string name, string guess)
            {
                Name = name;
                Guess = guess;
            }
        }
    }
}