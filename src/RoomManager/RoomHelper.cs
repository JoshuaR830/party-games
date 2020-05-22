using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class RoomHelper: IRoomHelper
    {
        private readonly Dictionary<string, Room> _rooms;

        public RoomHelper()
        {
            _rooms = Rooms.RoomsList;
        }
        
        public void CreateRoom(string name, string roomId)
        {
            if (!_rooms.ContainsKey(roomId))
                Rooms.CreateRoom(name, roomId);

            if(!_rooms[roomId].Users.ContainsKey(name))
                _rooms[roomId].AddUser(name);
        }

        public void AddToScore(int scoreToAdd)
        {
            throw new System.NotImplementedException();
        }

        public void SetScore(int newScore)
        {
            throw new System.NotImplementedException();
        }
    }
}