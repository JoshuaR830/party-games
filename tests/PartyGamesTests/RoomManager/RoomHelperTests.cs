using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager
{
    public class RoomHelperTests
    {
        [Fact]
        public void WhenAUserJoinsARoomThenTheUserShouldBeAddedToTheRoom()
        {
            var roomHelper = new RoomHelper();

            var name = "Joshua";
            var roomId = "NewGroup";
            
            roomHelper.CreateRoom(name, roomId);

            var rooms = Rooms.GetRoomsList();
            
            rooms.Should().HaveCount(1);
            
            rooms
                .Should()
                .ContainKey("NewGroup")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
        }
        
        [Fact]
        public void WhenMultipleUsersJoinARoomUserJoinsARoomThenTheUserShouldBeAddedToTheRoom()
        {
            var roomHelper = new RoomHelper();
          
            roomHelper.CreateRoom("Joshua", "NewGroup");
            roomHelper.CreateRoom("Lydia", "NewGroup");
            roomHelper.CreateRoom("Andrew", "NewGroup");
            roomHelper.CreateRoom("Kerry", "NewGroup");

            var rooms = Rooms.GetRoomsList();
            
            rooms.Should().HaveCount(4);
            
            rooms
                .Should()
                .ContainKey("NewGroup")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
            
            rooms
                .Should()
                .ContainKey("NewGroup")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Andrew")
                .WhichValue
                .Name
                .Should()
                .Be("Andrew");
            
            rooms
                .Should()
                .ContainKey("NewGroup")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Kerry")
                .WhichValue
                .Name
                .Should()
                .Be("Kerry");
            
            rooms
                .Should()
                .ContainKey("NewGroup")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Lydia")
                .WhichValue
                .Name
                .Should()
                .Be("Lydia");
        }
        
        [Fact]
        public void WhenUsersJoinDifferentRoomsThenTheUserShouldBeAddedToTheCorrectRoom()
        {
            var roomHelper = new RoomHelper();
          
            roomHelper.CreateRoom("Joshua", "NewGroup4");
            roomHelper.CreateRoom("Lydia", "NewGroup1");
            roomHelper.CreateRoom("Andrew", "NewGroup2");
            roomHelper.CreateRoom("Kerry", "NewGroup3");

            var rooms = Rooms.GetRoomsList();
            
            rooms.Should().HaveCount(4);
            
            rooms
                .Should()
                .ContainKey("NewGroup1")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Lydia")
                .WhichValue
                .Name
                .Should()
                .Be("Lydia");
            
            rooms
                .Should()
                .ContainKey("NewGroup2")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Andrew")
                .WhichValue
                .Name
                .Should()
                .Be("Andrew");
            
            rooms
                .Should()
                .ContainKey("NewGroup3")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Kerry")
                .WhichValue
                .Name
                .Should()
                .Be("Kerry");
            
            rooms
                .Should()
                .ContainKey("NewGroup4")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
        }

        [Fact]
        public void WhenAUserWithTheSameNameJoinsDifferentRoomsThenEachRoomSHouldHaveADifferentUserWithTheSameName()
        {
            var roomHelper = new RoomHelper();
          
            roomHelper.CreateRoom("Joshua", "NewGroup1");
            roomHelper.CreateRoom("Joshua", "NewGroup2");
            roomHelper.CreateRoom("Joshua", "NewGroup3");
            roomHelper.CreateRoom("Joshua", "NewGroup4");

            var rooms = Rooms.GetRoomsList();
            
            rooms.Should().HaveCount(4);
            
            rooms
                .Should()
                .ContainKey("NewGroup1")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
            
            rooms
                .Should()
                .ContainKey("NewGroup2")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
            
            rooms
                .Should()
                .ContainKey("NewGroup3")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
            
            rooms
                .Should()
                .ContainKey("NewGroup4")
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
        }
    }
}