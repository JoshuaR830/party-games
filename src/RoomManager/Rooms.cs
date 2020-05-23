using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class Rooms
    {
        private static readonly Rooms RoomsInstance = new Rooms();
        public static Dictionary<string, Room> RoomsList { get; private set; }

        private Rooms()
        {
            RoomsList = new Dictionary<string, Room>();
        }

        public static Rooms GetRooms()
        {
            return RoomsInstance;
        }

        public static void CreateRoom(string name, string roomId)
        {
            RoomsList.Add(roomId, new Room());
        }

        public static void DeleteRoom(string roomId)
        {
            RoomsList.Remove(roomId);
        }
    }
}