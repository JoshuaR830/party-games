using System.Collections.Generic;

namespace Chat.RoomManager
{
    public interface IScoreHelper
    {
        int CalculateThoughtsAndCrossesScore(List<bool> scoresList);
    }
}