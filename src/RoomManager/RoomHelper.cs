using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class RoomHelper: IRoomHelper
    {
        public void CreateRoom(string name, string roomId)
        {
            var rooms = Rooms.RoomsList;
            if (!rooms.ContainsKey(roomId))
                Rooms.CreateRoom(name, roomId);

            if(!rooms[roomId].Users.ContainsKey(name))
                rooms[roomId].AddUser(name);
        }
        
        
        
    }
}