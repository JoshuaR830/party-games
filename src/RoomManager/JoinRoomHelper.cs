using System.Collections.Concurrent;

namespace Chat.RoomManager
{
    public class JoinRoomHelper : IJoinRoomHelper
    {
        private readonly ConcurrentDictionary<string, Room> _rooms;

        private readonly string _name;
        private readonly string _roomId;
        
        public JoinRoomHelper()
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
    }
}