using System;
using System.Linq;
using System.Threading.Tasks;
using Chat.GameManager;
using Chat.RoomManager;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Chat.Hubs
{
    public class PixenaryHub : Hub
    {
        private readonly IGameManager _gameManager;

        public PixenaryHub(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        // Need to add user to group
        public async Task JoinPixenaryGame(string roomId, string userId, int gameId)
        {
            if (!Rooms.RoomsList.ContainsKey(roomId))
            {
                _gameManager.SetupNewGame(roomId, userId, (GameType) gameId);
            }

            if (Rooms.RoomsList[roomId].PixenaryGame == null)
            {
                _gameManager.SetupNewGame(roomId, userId, (GameType) gameId);
            }
            
            Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList[roomId].Users.Values.Select(x => x.Name).ToList()));

            var game = Rooms.RoomsList[roomId].PixenaryGame;

            if (game == null)
                return;

            if (!Rooms.RoomsList[roomId].Users.ContainsKey(userId))
                _gameManager.SetUpNewPixenaryUser(roomId, userId, game);

            if (Rooms.RoomsList[roomId].Users[userId].PixenaryGame == null)
                _gameManager.SetUpNewPixenaryUser(roomId, userId, game);
            
            Rooms.RoomsList[roomId].PixenaryGame.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Values.Select(x => x.Name).ToList());
            
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            Console.WriteLine("Joined game");
            // await Clients.Group(userId).SendAsync("PixelGridResponse", JsonConvert.SerializeObject(game.Grid), true);
            //
            // Rooms.RoomsList[roomId].PixenaryGame.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Values.Select(x => x.Name).ToList());
            //
            // if (userId == Rooms.RoomsList[roomId].PixenaryGame.ActivePlayer)
            //     await Clients.Group(userId).SendAsync("PixelWord", Rooms.RoomsList[roomId].PixenaryGame.Word);
        }

        public async Task StartPixenary(string roomId)
        {
            
            var game = Rooms.RoomsList[roomId].PixenaryGame;
            Rooms.RoomsList[roomId].PixenaryGame.ChooseActivePlayer();
            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                var userId = user.Value.Name;
                
                
                var playerTurn = Rooms.RoomsList[roomId].PixenaryGame.ActivePlayer;
                await Clients.Group(userId).SendAsync("PixelGridResponse", JsonConvert.SerializeObject(game.Grid), playerTurn == userId);
                // Rooms.RoomsList[roomId].PixenaryGame.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Values.Select(x => x.Name).ToList());

                Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList[roomId].Users.Values.Select(x => x.Name).ToList()));

                Console.WriteLine(Rooms.RoomsList[roomId].PixenaryGame.ActivePlayer);
            }
            
            await Clients.Group(Rooms.RoomsList[roomId].PixenaryGame.ActivePlayer).SendAsync("PixelWord", Rooms.RoomsList[roomId].PixenaryGame.Word);
        }

        public async Task UpdatePixelGrid(string roomId, int pixelPosition, string pixelColor)
        {
            Rooms.RoomsList[roomId].PixenaryGame.UpdatePixel(pixelPosition, pixelColor);
            await Clients.Group(roomId).SendAsync("PixelGridUpdate", pixelPosition, pixelColor);
        }

        public async Task ResetPixenary(string roomId)
        {
            Rooms.RoomsList[roomId].PixenaryGame.ResetGame();
            
            await Clients.Group(roomId).SendAsync("ResetGame");

            foreach (var user in Rooms.RoomsList[roomId].Users)
            {
                var userId = user.Value.Name;
                
                // var game = Rooms.RoomsList[roomId].PixenaryGame;

                // await Clients.Group(userId).SendAsync("PixelGridResponse", JsonConvert.SerializeObject(game.Grid), true);
                // Rooms.RoomsList[roomId].PixenaryGame.AddPlayersToGame(Rooms.RoomsList[roomId].Users.Values.Select(x => x.Name).ToList());
                Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList[roomId].Users.Values.Select(x => x.Name).ToList()));

                // if (userId == Rooms.RoomsList[roomId].PixenaryGame.ActivePlayer)
                //     await Clients.Group(userId).SendAsync("PixelWord", Rooms.RoomsList[roomId].PixenaryGame.Word);
            }
            
            _gameManager.SetupNewGame(roomId, Rooms.RoomsList[roomId].Users.First().Key, GameType.Pixenary);

            Console.WriteLine(JsonConvert.SerializeObject(Rooms.RoomsList[roomId].PixenaryGame.WordsWithCategories.Select(x => x.Word)));
            
            await StartPixenary(roomId);
        }

        public async Task SendColors(string roomId, string[] colorList)
        {
            await Clients.Group(roomId).SendAsync("ReceiveColors", colorList);
        }

        // Need to be able to reset the round
        // Need to be able to update the list
        // Need to be able to send the changed item and colours to all players
        // Need to be able to request the list on load
    }
}