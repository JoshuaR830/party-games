using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.RoomManager
{
    public class ShuffleHelper<T> : IShuffleHelper<T>
    {
        public List<T> ShuffleList(List<T> list)
        {
            var rand = new Random();
            
            if (list.Count <= 1)
                return list;
            
            var shuffledList = new List<T>();
            do
            {
                shuffledList = list.OrderBy(x => rand.Next()).ToList();
            } while (list.SequenceEqual(shuffledList));

            return shuffledList;
        }
    }
}