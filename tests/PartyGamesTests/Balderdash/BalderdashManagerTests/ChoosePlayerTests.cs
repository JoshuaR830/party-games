using System.Collections.Generic;
using System.Linq;
using Chat.Balderdash;
using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.Balderdash.BalderdashManagerTests
{
    public class ChoosePlayerTests
    {
        public ChoosePlayerTests()
        {
            var room = new Room();
            var shuffleHelper = new ShuffleHelper<string>();
            
            var game = new BalderdashManager(shuffleHelper);
            room.SetBalderdashGame(game);
            
            
            Rooms.RoomsList.TryAdd("ChoosePlayer", room);
            
            var user = new User("Joshua");
            var userGame = new UserBalderdashGame();
            user.SetUpGame(userGame);
            
            Rooms.RoomsList["ChoosePlayer"].Users.TryAdd("Joshua", new User("Joshua"));
            Rooms.RoomsList["ChoosePlayer"].Users.TryAdd("Kerry", new User("Kerry"));
            Rooms.RoomsList["ChoosePlayer"].Users.TryAdd("Andrew", new User("Andrew"));
            Rooms.RoomsList["ChoosePlayer"].Users.TryAdd("Lydia", new User("Lydia"));

            var users = Rooms.RoomsList["ChoosePlayer"].Users.Select(x => x.Key);
            Rooms.RoomsList["ChoosePlayer"].Balderdash.AddPlayersToGame(users);
        }

        [Fact]
        public void ThePlayersShouldBeSet()
        {
            Rooms.RoomsList["ChoosePlayer"].Balderdash.PlayerOrder.Should().NotBeNull();
            Rooms.RoomsList["ChoosePlayer"].Balderdash.PlayerOrder.Should().HaveCount(4);
        }

        [Fact]
        public void TheOrderShouldBeRandomTheFirstTime()
        {
            Rooms.RoomsList["ChoosePlayer"].Balderdash.SetPlayerOrder();
            var playerOrder1 = new string[4];
            var playerOrder2 = new string[4];
            
            Rooms.RoomsList["ChoosePlayer"].Balderdash.PlayerOrder.CopyTo(playerOrder1);
            Rooms.RoomsList["ChoosePlayer"].Balderdash.SetPlayerOrder();
            Rooms.RoomsList["ChoosePlayer"].Balderdash.PlayerOrder.CopyTo(playerOrder2);

            Rooms.RoomsList["ChoosePlayer"].Balderdash.PlayerOrder.Should().NotBeNullOrEmpty();
            playerOrder1.Should().NotEqual(playerOrder2);
        }

        [Fact]
        public void TheOrderShouldBeTheSameAfterTheFirstTime()
        {
            var playerOrder = Rooms.RoomsList["ChoosePlayer"].Balderdash.PlayerOrder;
            
            for (var j = 0; j < 3; j++)
            {
                for (var i = 0; i < playerOrder.Count; i++)
                {
                    var player = playerOrder[i];
                    Rooms.RoomsList["ChoosePlayer"].Balderdash.SelectPlayer();
                    Rooms.RoomsList["ChoosePlayer"].Balderdash.SelectedPlayer.Should().Be(player);
                }
            }
        }
    }
}
