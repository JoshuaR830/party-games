using System;
using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager.RoomHelperTests
{
    public class JoinTests : IDisposable
    {
        private readonly JoinRoomHelper _joinRoomHelper;

        public JoinTests()
        {
            Rooms.DeleteRooms();
            _joinRoomHelper = new JoinRoomHelper();
        }
        
        [Fact]
        public void WhenAUserJoinsARoomThenTheUserShouldBeAddedToTheRoom()
        {
            var name = "Joshua";
            var roomId = "NewGroup";
            
            _joinRoomHelper.CreateRoom(name, roomId);

            var rooms = Rooms.RoomsList;
            
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
        public void WhenTheSameUserTriesToJoinTheRoomTwiceThenTheDictionaryShouldOnlyBeUpdatedOnce()
        {
            var name = "Joshua";
            var roomId = "NewGroup";
            
            _joinRoomHelper.CreateRoom(name, roomId);
            _joinRoomHelper.CreateRoom(name, roomId);

            var rooms = Rooms.RoomsList;
            
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
            _joinRoomHelper.CreateRoom("Joshua", "NewGroup");
            _joinRoomHelper.CreateRoom("Lydia", "NewGroup");
            _joinRoomHelper.CreateRoom("Andrew", "NewGroup");
            _joinRoomHelper.CreateRoom("Kerry", "NewGroup");

            var rooms = Rooms.RoomsList;
            
            rooms.Should().HaveCount(1);
            rooms["NewGroup"].Users.Should().HaveCount(4);
            
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
            _joinRoomHelper.CreateRoom("Joshua", "NewGroup4");
            _joinRoomHelper.CreateRoom("Lydia", "NewGroup1");
            _joinRoomHelper.CreateRoom("Andrew", "NewGroup2");
            _joinRoomHelper.CreateRoom("Kerry", "NewGroup3");

            var rooms = Rooms.RoomsList;
            
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
        public void WhenAUserWithTheSameNameJoinsDifferentRoomsThenEachRoomShouldHaveADifferentUserWithTheSameName()
        {
            _joinRoomHelper.CreateRoom("Joshua", "NewGroup1");
            _joinRoomHelper.CreateRoom("Joshua", "NewGroup2");
            _joinRoomHelper.CreateRoom("Joshua", "NewGroup3");
            _joinRoomHelper.CreateRoom("Joshua", "NewGroup4");

            var rooms = Rooms.RoomsList;
            
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

        public void Dispose()
        {
            Rooms.DeleteRooms();
        }
    }
}