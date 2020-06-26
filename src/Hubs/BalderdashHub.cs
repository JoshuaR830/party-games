using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chat.GameManager;
using Chat.RoomManager;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Chat.Hubs
{
    public class BalderdashHub : Hub
    {
        
        private readonly IGameManager _gameManager;

        public BalderdashHub(IGameManager gameManager)
        {
            _gameManager = gameManager;
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
                _gameManager.SetupNewGame(roomId, name, GameType.Word);
                Rooms.RoomsList[roomId].Word.GetLetters();
                Console.WriteLine(name);
            }

            Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList));
            Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList[roomId].Users));
            
            // Todo: This is the wrong game type  
            if (!Rooms.RoomsList[roomId].Users.ContainsKey(name))
            {
                var game = Rooms.RoomsList[roomId].Word;
                _gameManager.SetUpNewWordUser(roomId, name, game);
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

        public async Task BalderdashScores()
        {
            // ToDo: need to take who is giving answer and who they chose
            // ToDo: server needs to work out the scores
        }

        public async Task BalderdashDiscardSimilar()
        {
            // ToDo: if number discarded = 2 start next round
            // ToDo: call reset game
        }

        public async Task ResetGame()
        {
            // ToDo: keep scores, keep player order but update dasher and clear screens
        }

        public async Task PassToDasher(string roomId, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, name);

            var guesses = new List<GuessMade>
            {
                new GuessMade("Joshua", "Joshua's guess"),
                new GuessMade("Lydia", "Lydia's guess"),
                new GuessMade("Andrew", "Andrew's guess"),
                new GuessMade("Kerry", "Kerry's guess")
            };
            
            await Clients.Group(name).SendAsync("ReceivedGuesses", guesses);
            
            

            // ToDo: send text and who
            // ToDo: server knows who dasher is so can send it easily
            // ToDo: keep a record of all of a rounds guesses - send all back to the dasher in random order
            // ToDo: object would look like {"User": "Name", "Data": "This is my guess"}
            // ToDo: When user presses submit it will send to the server
            // ToDo: Keep track of the number of guesses in server 
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