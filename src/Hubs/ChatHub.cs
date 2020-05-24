using System;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Chat.GameManager;
using Chat.RoomManager;
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
        public async Task JoinRoom(string roomId, string name, int gameId)
        {
            _gameManager.SetupGame(roomId, name, (GameType) gameId);
            Console.WriteLine("Hello");

            var userWordGrid = Rooms.RoomsList[roomId].Users[name].UserThoughtsAndCrosses.WordsGrid;
            Console.WriteLine(JsonConvert.SerializeObject(userWordGrid));
            // await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ReceiveWordGrid", userWordGrid);
        }
        
    }
}
