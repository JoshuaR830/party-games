using System.Threading.Tasks;
using Chat.GameManager;
using Chat.RoomManager;
using Microsoft.AspNetCore.SignalR;

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
           
            var game = Rooms.RoomsList[roomId].PixenaryGame;

            if (game == null)
                return;

            if (!Rooms.RoomsList[roomId].Users.ContainsKey(userId))
                _gameManager.SetUpNewPixenaryUser(roomId, userId, game);

            if (Rooms.RoomsList[roomId].Users[userId].PixenaryGame == null)
                _gameManager.SetUpNewPixenaryUser(roomId, userId, game);

            
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            
            await Clients.Group(userId).SendAsync("PixelGridResponse", game.Grid);
        }

        public async Task UpdatePixelGrid(string roomId, int pixelPosition, string pixelColor)
        {
            Rooms.RoomsList[roomId].PixenaryGame.UpdatePixel(pixelPosition, pixelColor);
            await Clients.Group(roomId).SendAsync("PixelGridUpdate", pixelPosition, pixelColor);
        }

        // Need to be able to reset the round
        // Need to be able to update the list
        // Need to be able to send the changed item and colours to all players
        // Need to be able to request the list on load
    }
}