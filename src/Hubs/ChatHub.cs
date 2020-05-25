using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Chat.GameManager;
using Chat.RoomManager;
using Microsoft.AspNetCore.Mvc.Internal;
using Newtonsoft.Json;

namespace Chat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IGameManager _gameManager;

        public ChatHub(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task StartGame(string user, string letter, int[] time, string topics)
        {
            System.Console.WriteLine("Start\n\n");
            await Clients.Group(user).SendAsync("ReceiveLetter", letter);
            await Clients.Group(user).SendAsync("ReceiveTopics", Context.ConnectionId, user, topics);
            await Clients.Group(user).SendAsync("ReceiveTimeStart", time);
            await Clients.Group(user).SendAsync("StartNewRound");
        }

        public async Task SendMessage(string user, string message)
        {
            // await SendDirectMessage("my group", "user", message);
            Console.WriteLine("Indirect");

            await Clients.Group(user).SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
        }

        public async Task SendLetter(string letter, string user)
        {
            System.Console.WriteLine("Recieved Letter\n\n");
            await Clients.Group(user).SendAsync("ReceiveLetter", letter);
        }

        public async Task SendScore(string user, string name, string score)
        {
            var message = $"{name}: {score}";
            // await SendDirectMessage("my group", "user", message);
            Console.WriteLine("Indirect");

            await Clients.Group(user).SendAsync("ReceiveMessage", Context.ConnectionId, user, message);
        }
        
        public async Task CollectScores(string user)
        {
            await Clients.Group(user).SendAsync("CompletedScores");
        }

        public async Task SendTopics(string user, string message)
        {
            // await SendDirectMessage("my group", "user", message);
            Console.WriteLine("Indirect");

            await Clients.Group(user).SendAsync("ReceiveTopics", Context.ConnectionId, user, message);
        }

        public async Task SendTime(string user, int[] time)
        {
            // await SendDirectMessage("my group", "user", message);
            Console.WriteLine("Indirect");

            await Clients.Group(user).SendAsync("ReceiveTimeStart", time);
        }

        public async Task StopTimer(string user)
        {
            // await SendDirectMessage("my group", "user", message);
            Console.WriteLine("Indirect");

            await Clients.Group(user).SendAsync("ReceiveStopTimer");
        }

        public async Task CompleteRound(string user)
        {
            await Clients.Group(user).SendAsync("ReceiveCompleteRound");
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            Console.WriteLine(groupName + " hello there");
            var friendlyName = groupName;
        }

        public async Task AddToAdminGroup(string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            Console.WriteLine("Connected");
            await Clients.Group("admin").SendAsync("AdminConnected", name);
        }

        public async Task AddUserToDashboard(string user)
        {
            Console.WriteLine(">>> Hello");
            await Groups.AddToGroupAsync(Context.ConnectionId, "admin");
            await Clients.Group("admin").SendAsync("UserConnected", user);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", Context.ConnectionId, groupName, $"Left the group: {groupName}");
        }

        public async Task SendDirectMessage(string recipient, string myName, string message)
        {
            Console.WriteLine("Direct");
            System.Console.WriteLine(recipient);
            System.Console.WriteLine(myName);
            System.Console.WriteLine(message);
            await Clients.Group(recipient).SendAsync("ReceiveDirectMessage", recipient, myName, message);
        }

        
        // Server side processing

        public async Task Startup(string roomId, string name, int gameId)
        {
            Console.WriteLine("New room 1");
            await Groups.AddToGroupAsync(Context.ConnectionId, name);

            if (!Rooms.RoomsList.ContainsKey(roomId))
            {
                Console.WriteLine("New room 2");
                _gameManager.SetupNewGame(roomId, name, (GameType) gameId);
                Rooms.RoomsList[roomId].ThoughtsAndCrosses.SetLetter();
                Console.WriteLine(name);
            }
            
            if (Rooms.RoomsList[roomId].WordGame == null)
            {
                _gameManager.SetupNewGame(roomId, name, (GameType) gameId);
                if (Rooms.RoomsList[roomId].WordGame == null)
                    return;
                
                Rooms.RoomsList[roomId].WordGame.GetLetters();
            }
        }

        public void SetupNewUser(string roomId, string name)
        {
            Console.WriteLine("New user");
            if (!Rooms.RoomsList[roomId].Users.ContainsKey(name))
            {
                var game = Rooms.RoomsList[roomId].ThoughtsAndCrosses;
                _gameManager.SetupNewThoughtsAndCrossesUser(roomId, name, game);
                Console.WriteLine(name);
            }
            
            if (Rooms.RoomsList[roomId].Users[name].WordGame == null)
            {
                var game = Rooms.RoomsList[roomId].WordGame;
                _gameManager.SetUpNewWordGameUser(roomId, name, game);
            }
        }
        
        public async Task StartServerGame(string user, int[] time)
        {
            await Clients.Group(user).SendAsync("ReceiveTimeStart", time);
            await Clients.Group(user).SendAsync("StartNewRound");
        }
        
        public async Task JoinRoom(string roomId, string name, int gameId)
        {
            Console.WriteLine("Join room");
            // var userWordGrid = Rooms.RoomsList[roomId].Users[name].UserThoughtsAndCrosses.WordsGrid;
            // Console.WriteLine(JsonConvert.SerializeObject(userWordGrid));
            // await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            var letter = Rooms.RoomsList[roomId].ThoughtsAndCrosses.Letter.Letter;

            await Clients.Group(roomId).SendAsync("ReceiveLetter", letter);

            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                Console.WriteLine(user.Value.ThoughtsAndCrosses.WordsGrid);
                Console.WriteLine(user.Value.Name);
                await Clients.Group(user.Value.Name).SendAsync("ReceiveWordGrid", user.Value.ThoughtsAndCrosses.WordsGrid);
            }
        }

        public async Task SetGuessForCategory(string roomId, string name, string category, string userGuess)
        {
            Console.WriteLine(userGuess);
            Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrosses.ManageGuess(category, userGuess);   
        }
        
        public async Task SetIsValidForCategory(string roomId, string name, string category, bool isValid)
        {
            if (isValid)
            {
                Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrosses.CheckWord(category);
            }
            else
            {
                Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrosses.UncheckWord(category);
            }
        }

        public async Task CalculateScore(string roomId, string name)
        {
            Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrosses.CalculateScore();
            var score = Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrosses.Score;
            await Clients.Group(name).SendAsync("ScoreCalculated", score);
        }

        public async Task ResetGame(string roomId, string name, int gameId)
        {
            var game = Rooms.RoomsList[roomId].ThoughtsAndCrosses;
            _gameManager.ResetThoughtsAndCrosses(roomId, game);
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                _gameManager.ResetThoughtsAndCrosssesForUser(roomId, user.Value.Name, game);
            }
            
            await JoinRoom(roomId, name, gameId);
        }
        
        public async Task UpdateScoreBoard(string roomId, string name)
        {
            var score = Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrosses.Score;
            var message = $"{name}: {score}";
            // await SendDirectMessage("my group", "user", message);
            Console.WriteLine("Indirect");

            await Clients.Group(roomId).SendAsync("ReceiveMessage", Context.ConnectionId, roomId, message);
        }
        
    }
}
