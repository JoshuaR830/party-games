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
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
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
                SetUpUser(roomId, name);
            }

            Rooms.RoomsList[roomId].Balderdash.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Keys);
            Rooms.RoomsList[roomId].Balderdash.Reset();

            var loggedInUsers = Rooms.RoomsList[roomId].Users.Select(x => x.Key).ToList().OrderBy(x => x); 
            await Clients.Group(roomId).SendAsync("LoggedInUsers", loggedInUsers);
            // ToDo: add the game to the server and set up anything that needs to be set
            // ToDo: Trigger off the back of login
        }

        public async Task SetUpUser(string roomId, string userId)
        {
            // ToDo: connect user name to server
            // ToDO: connect user to group server for group updates
            
            // ToDo: add user to the game list
            var game = Rooms.RoomsList[roomId].Balderdash;
            _gameManager.SetUpNewBalderdashUser(roomId, userId, game);
            Rooms.RoomsList[roomId].Balderdash.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Select(x => x.Key));
            Console.WriteLine(userId);

            Rooms.RoomsList[roomId].Balderdash.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Keys);
            Rooms.RoomsList[roomId].Balderdash.SetPlayerOrder();
        }

        public async Task BeginBalderdash(string roomId)
        {
            await ResetGame(roomId);
        }

        public async Task BalderdashScores(string roomId, string playerWhoGuessed, string playerWhoProposed)
        {
            var room = Rooms.RoomsList[roomId];
            var users = room.Users;
            var balderdash = room.Balderdash;

            users[playerWhoGuessed].BalderdashGame.MadeGuessThisRound();
            
            _balderdashScoreCalculator.CalculateGuesser(roomId, playerWhoGuessed, playerWhoProposed);
            _balderdashScoreCalculator.CalculateProposer(roomId, playerWhoGuessed, playerWhoProposed);

            if (playerWhoProposed == Rooms.RoomsList[roomId].Balderdash.SelectedPlayer)
            {
                Rooms.RoomsList[roomId].Balderdash.SetIsDasherGuessed(true);
            }
            

            var shouldDasherGetPoint = users
                .Where(x => x.Key != balderdash.SelectedPlayer)
                .All(x => x.Value.BalderdashGame.HasMadeGuessThisRound);
            
            if (shouldDasherGetPoint)
                _balderdashScoreCalculator.CalculateDasherScore(roomId, Rooms.RoomsList[roomId].Balderdash.SelectedPlayer);
        }

        public async Task BalderdashDiscardSimilar()
        {
            // ToDo: if number discarded = 2 start next round
            // ToDo: call reset game
            
            // Todo: have a property - discarded this round
            // ToDo: update score if user gets theirs discarded
        }

        public async Task DisplayRoundInformation(string roomId)
        {
            var room = Rooms.RoomsList[roomId];
            var userRoundScores = new Dictionary<string, int>();
            
            foreach (var user in room.Users)
            {
                userRoundScores.Add(user.Key, user.Value.BalderdashGame.RoundScore);
            }

            await Clients.Group(roomId).SendAsync("RoundReviewPage", userRoundScores);
        }

        public async Task ResetGame(string roomId)
        {
            var room = Rooms.RoomsList[roomId];
            var users = room.Users;

            foreach (var user in users)
            {
                if (user.Value.BalderdashGame == null)
                    SetUpUser(roomId, user.Key);

                user.Value.BalderdashGame?.Reset();

            }
            
            room.Balderdash.Reset();
            await Clients.Group(roomId).SendAsync("Reset");

            var dasher = Rooms.RoomsList[roomId].Balderdash.SelectedPlayer;
            await Clients.Group(roomId).SendAsync("DasherSelected", dasher);
            
            // ToDo: synchronise deletion by sending update to all devices
        }
        
        public async Task DisplayBalderdashScores(string roomId, string name)
        {
            var users = Rooms.RoomsList[roomId].Users;
            var dasher = Rooms.RoomsList[roomId].Balderdash.SelectedPlayer;
            var names = users.Select(x => x.Key).Where(y => y != dasher).ToList().OrderBy(x => x);
            await Clients.Group(roomId).SendAsync("DisplayBalderdashScores", names);
        }

        public async Task GetScoresForAllUsers(string roomId)
        {
            var users = Rooms.RoomsList[roomId].Users;

            foreach (var user in users)
            {
                var score = user.Value.BalderdashGame.Score;
                await Clients.Group(user.Key).SendAsync("UpdateUserScore", score);
            }
        }

        public async Task PassToDasher(string roomId, string name, string guess)
        {
            Rooms.RoomsList[roomId].Users[name].BalderdashGame.SetGuess(guess);

            
            // ToDo: remove fake setup
            // FakeSetup(roomId);
            
            
            var guesses = new List<GuessMade>();
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                guesses.Add(new GuessMade(user.Key, user.Value.BalderdashGame.Guess));
            }

            guesses = _shuffleGameOrder.ShuffleList(guesses);

            var dasher = Rooms.RoomsList[roomId].Balderdash.SelectedPlayer;
            await Clients.Group(name).SendAsync("ReceivedGuess", dasher);
                
            if(Rooms.RoomsList[roomId].Users.Any(x => x.Value.BalderdashGame.Guess == ""))
                return;

            var guessesMade = Rooms.RoomsList[roomId].Users.Select(x => new GuessMade(x.Key, x.Value.BalderdashGame.Guess)).ToList();
            guessesMade = _shuffleGameOrder.ShuffleList(guessesMade);
            Console.WriteLine(JsonConvert.SerializeObject(guessesMade));
            await Clients.Group(dasher).SendAsync("RevealCardsToDasher", guessesMade);
            
            

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