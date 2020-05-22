using System.Collections.Generic;
using Chat.WordGame.LocalDictionaryHelpers;

namespace Chat.RoomManager
{
    public class Rooms
    {
        private static readonly Rooms RoomsInstance = new Rooms();
        public static Dictionary<string, Room> RoomsList { get; set; }

        private Rooms()
        {
            RoomsList = new Dictionary<string, Room>();
        }

        public static Rooms GetRooms()
        {
            return RoomsInstance;
        }

        public static Dictionary<string, Room> GetRoomsList()
        {
            return RoomsList;
        }
    }
}