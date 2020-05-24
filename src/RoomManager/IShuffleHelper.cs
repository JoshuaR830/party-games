using System.Collections.Generic;

namespace Chat.RoomManager
{
    public interface IShuffleHelper<T>
    {
        List<T> ShuffleList(List<T> list);
    }
}