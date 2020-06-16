using System.Collections.Generic;

namespace Chat.RoomManager
{
    public class RoomHelper: IRoomHelper
    {
        private readonly Dictionary<string, Room> _rooms;

        private readonly string _name;
        private readonly string _roomId;
        
        public RoomHelper(string name, string roomId)
        {
            _rooms = Rooms.RoomsList;
            _name = name;
            _roomId = roomId;
        }

        public void AddToScore(int scoreToAdd)
        {
            var score = _rooms[_roomId].Users[_name].Score + scoreToAdd;
            SetScore(score);
        }

        public void SetScore(int newScore)
        {
            _rooms[_roomId].Users[_name].SetScore(newScore);
        }
    }
}