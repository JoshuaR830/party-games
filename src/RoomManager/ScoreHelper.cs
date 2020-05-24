using System.Collections.Generic;
using System.Linq;

namespace Chat.RoomManager
{
    public class ScoreHelper : IScoreHelper
    {
        public List<bool> ScoresList { get; private set; }

        public int CalculateThoughtsAndCrossesScore(List<bool> scoresList)
        {
            ScoresList = scoresList;

            var score = CalculateIndividualCellScore();
            score += CalculateHorizontals();
            score += CalculateVerticals();
            score += CalculateDiagonals();

            return score;
        }

        private int CalculateDiagonals()
        {
            var score = 0;
            if (ScoresList[0] && ScoresList[4] && ScoresList[8])
                score += 3;

            if (ScoresList[2] && ScoresList[4] && ScoresList[6])
                score += 3;

            return score;
        }

        private int CalculateVerticals()
        {
            var score = 0;
            for (var x = 0; x < 3; x++)
            {
                var temp = 0;
                for (var y = 0; y < 9; y += 3)
                {
                    if (ScoresList[y + x])
                        temp++;
                }

                if (temp == 3)
                    score += 3;
            }

            return score;
        }

        private int CalculateHorizontals()
        {
            var score = 0;
            for (var y = 0; y < 9; y += 3)
            {
                var temp = 0;

                for (var x = 0; x < 3; x++)
                {
                    if(ScoresList[y + x])
                        temp++;
                }

                if (temp == 3)
                    score += 3;
            }

            return score;
        }

        private int CalculateIndividualCellScore()
        {
            return ScoresList.Count(x => x == true);
        }
    }
}