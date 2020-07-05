using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Chat.GameManager;
using Chat.RoomManager;
using Microsoft.AspNetCore.Mvc.Internal;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.Extensions.Logging.Console;

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

            Console.WriteLine(user);
            Console.WriteLine(message);

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
                // The problem here is that this is getting invoked to build pixenary - but this requires a ThoughtsAndCrosses game to be made
                Console.WriteLine("New room 2");
                _gameManager.SetupNewGame(roomId, name, (GameType) gameId);
                Rooms.RoomsList[roomId].ThoughtsAndCrosses.SetLetter();
                Console.WriteLine(name);
            }
            
            if (Rooms.RoomsList[roomId].ThoughtsAndCrosses == null)
            {
                _gameManager.SetupNewGame(roomId, name, (GameType) gameId);
                if (Rooms.RoomsList[roomId].ThoughtsAndCrosses == null)
                    return;
                
                Rooms.RoomsList[roomId].ThoughtsAndCrosses.CalculateTopics();
                Rooms.RoomsList[roomId].ThoughtsAndCrosses.SetLetter();
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
            
            if (Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrossesGame == null)
            {
                var game = Rooms.RoomsList[roomId].ThoughtsAndCrosses;
                _gameManager.SetupNewThoughtsAndCrossesUser(roomId, name, game);
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
                Console.WriteLine(user.Value.ThoughtsAndCrossesGame.WordsGrid);
                Console.WriteLine(user.Value.Name);
                await Clients.Group(user.Value.Name).SendAsync("ReceiveWordGrid", user.Value.ThoughtsAndCrossesGame.WordsGrid);
            }
        }

        public async Task SetGuessForCategory(string roomId, string name, string category, string userGuess)
        {
            Console.WriteLine(userGuess);
            Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrossesGame.ManageGuess(category, userGuess);   
        }
        
        public async Task SetIsValidForCategory(string roomId, string name, string category, bool isValid)
        {
            if (isValid)
            {
                Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrossesGame.CheckWord(category);
            }
            else
            {
                Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrossesGame.UncheckWord(category);
            }
        }

        public async Task CalculateScore(string roomId, string name)
        {
            Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrossesGame.CalculateScore();
            var score = Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrossesGame.Score;
            await Clients.Group(name).SendAsync("ScoreCalculated", score);
        }

        public async Task ResetGame(string roomId, string name, int gameId)
        {
            var game = Rooms.RoomsList[roomId].ThoughtsAndCrosses;
            _gameManager.ResetThoughtsAndCrosses(roomId, game);
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                _gameManager.ResetThoughtsAndCrossesForUser(roomId, user.Value.Name, game);
            }
            
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                if (user.Value.WordGame == null)
                {
                    Console.WriteLine($"No thoughts and crosses game for {user.Key}");
                    SetupNewUser(roomId, user.Key);
                }
                else
                {
                    Console.WriteLine($"Yes there is a thoughts and crosses game for {user.Key}");
                    _gameManager.ResetThoughtsAndCrossesForUser(roomId, user.Value.Name, game);
                }
            }
            
            await JoinRoom(roomId, name, gameId);
        }
        
        public async Task UpdateScoreBoard(string roomId, string name)
        {
            var score = Rooms.RoomsList[roomId].Users[name].ThoughtsAndCrossesGame.Score;
            var message = $"{name}: {score}";
            // await SendDirectMessage("my group", "user", message);
            Console.WriteLine("Indirect");

            await Clients.Group(roomId).SendAsync("ReceiveMessage", Context.ConnectionId, roomId, message);
        }

        public async Task UpdateManualScore(string roomId, string myName, string name, GameType gameType)
        {
            // This was just a prototype
            // ToDo: put this in a function and test it
            var game = gameType.ToString();
            var users = new List<User>();
            users.Add(Rooms.RoomsList[roomId].Users[name]);
            users.Add(Rooms.RoomsList[roomId].Users[myName]);

            // Console.WriteLine(name);
            // Console.WriteLine(users[0].PixenaryGame.Score);
            // Console.WriteLine(myName);
            // Console.WriteLine(users[1].PixenaryGame.Score);

            foreach (var user in users)
            {
                if (user == null)
                    return;
                            
                var gameState = user.GetType()?.GetProperty($"{game}Game")?.GetValue(user);
    
                var method = gameState?.GetType().GetMethod("SetScore");
                method?.Invoke(gameState, new object[] {1});
    
                var gameObject = user.GetType()?.GetProperty($"{game}Game")?.GetValue(user);
                var score = gameObject?.GetType().GetProperty("Score")?.GetValue(gameObject);
    
                Console.WriteLine(score);
    
                await Clients.Group(user.Name).SendAsync("ManuallyIncrementedScore", score);
            }
            
        }

        public async Task DisplayScores(string roomId, string name, int gameId)
        {
            var users = Rooms.RoomsList[roomId].Users;
            var names = users.Select(x => x.Key).Where(y => y != name);
            await Clients.Caller.SendAsync("DisplayScores", names, gameId);
        }

        public async Task JoinForScores(string roomId, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
        }
    }
}
