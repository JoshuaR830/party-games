using Chat.RoomManager;

namespace Chat.Balderdash
{
    public class BalderdashScoreCalculator : IBalderdashScoreCalculator
    {
        public void CalculateGuesser(string roomId, string playerWhoGuessed, string playerWhoProposed)
        {
            var dasher = Rooms.RoomsList[roomId].Balderdash.SelectedPlayer;
            if (playerWhoProposed == dasher)
                Rooms.RoomsList[roomId].Users[playerWhoGuessed].BalderdashGame.SetScore(1);
        }

        public void CalculateProposer(string roomId, string playerWhoGuessed, string playerWhoProposed)
        {
            var dasher = Rooms.RoomsList[roomId].Balderdash.SelectedPlayer;

            if (playerWhoProposed != dasher)
                Rooms.RoomsList[roomId].Users[playerWhoProposed].BalderdashGame.SetScore(1);
        }

        public void CalculateDasherScore(string roomId, string dasher)
        {
            if (!Rooms.RoomsList[roomId].Balderdash.IsDasherGuessed)
                Rooms.RoomsList[roomId].Users[dasher].BalderdashGame.SetScore(1);
            // ToDo: If everyone has guessed (except dasher)
            // ToDo: has anyone guessed dasher?
            // ToDo: Have a bool for is dasher guessed - when scores submitted if false then give dasher a point
            // ToDo: reset the dasher guess on each round - don't want them to be guessed again
        }
    }
}