using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.RoomManager
{
    public class ShuffleHelper<T> : IShuffleHelper<T>
    {
        public List<T> ShuffleList(List<T> list)
        {
            var shuffledList = list;
            // ToDo: OrderBy random number
            return shuffledList;
        }
    }
}