using System;
using System.Collections.Concurrent;

namespace Chat.RoomManager
{
    public class Rooms
    {
        private static readonly Rooms RoomsInstance = new Rooms();
        public static ConcurrentDictionary<string, Room> RoomsList { get; private set; }

        private Rooms()
        {
            RoomsList = new ConcurrentDictionary<string, Room>();
        }

        public static Rooms GetRooms()
        {
            return RoomsInstance;
        }

        public static void CreateRoom(string name, string roomId)
        {
            Console.WriteLine($">>>{RoomsList}");
            RoomsList.TryAdd(roomId, new Room());
        }

        public static void DeleteRoom(string roomId)
        {
            RoomsList.TryRemove(roomId, out var room);
        }
    }
}