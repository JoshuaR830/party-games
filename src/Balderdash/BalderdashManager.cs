using System.Collections.Generic;
using Chat.RoomManager;

namespace Chat.Balderdash
{
    public class BalderdashManager : IBalderdashManager
    {
        private int _playerIndex = 0;
        readonly IShuffleHelper<string> _shuffleHelper;
        public List<string> PlayerOrder { get; private set; }
        public string SelectedPlayer { get; private set; }

        public BalderdashManager(IShuffleHelper<string> shuffleHelper)
        {
            _shuffleHelper = shuffleHelper;
            
            PlayerOrder = new List<string>();
        }
        
        public void AddPlayersToGame(IEnumerable<string> users)
        {
            PlayerOrder.Clear();
            foreach (var user in users)
            {
                PlayerOrder.Add(user);
            }
            
            PlayerOrder = _shuffleHelper.ShuffleList(PlayerOrder);
        }
        
        public void SetPlayerOrder()
        {
            PlayerOrder = _shuffleHelper.ShuffleList(PlayerOrder);
        }

        public void SelectPlayer()
        {
            if (_playerIndex >= PlayerOrder.Count)
                _playerIndex = 0;

            SelectedPlayer = PlayerOrder[_playerIndex];

            _playerIndex ++;
        }
    }
}