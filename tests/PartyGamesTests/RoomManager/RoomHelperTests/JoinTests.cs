using System;
using System.Collections.Generic;
using Chat.RoomManager;
using FluentAssertions;
using Xunit;

namespace PartyGamesTests.RoomManager.RoomHelperTests
{
    public class JoinTests : IDisposable
    {
        private readonly JoinRoomHelper _joinRoomHelper;
        private List<string> roomIds;

        public JoinTests()
        {
            roomIds = new List<string>();
            _joinRoomHelper = new JoinRoomHelper();
        }
        
        [Fact]
        public void WhenAUserJoinsARoomThenTheUserShouldBeAddedToTheRoom()
        {
            var name = "Joshua";
            var roomId = GenerateRoomName();

            _joinRoomHelper.CreateRoom(name, roomId);

            var rooms = Rooms.RoomsList;
            
            rooms
                .Should()
                .ContainKey(roomId)
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
            var roomId = GenerateRoomName();

            _joinRoomHelper.CreateRoom(name, roomId);
            _joinRoomHelper.CreateRoom(name, roomId);

            var rooms = Rooms.RoomsList;
            
            rooms
                .Should()
                .ContainKey(roomId)
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
            var roomId = GenerateRoomName();


            _joinRoomHelper.CreateRoom("Joshua", roomId);
            _joinRoomHelper.CreateRoom("Lydia", roomId);
            _joinRoomHelper.CreateRoom("Andrew", roomId);
            _joinRoomHelper.CreateRoom("Kerry", roomId);

            var rooms = Rooms.RoomsList;
            
            rooms
                .Should()
                .ContainKey(roomId)
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
                .ContainKey(roomId)
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
                .ContainKey(roomId)
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
                .ContainKey(roomId)
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
            var roomId1 = GenerateRoomName();
            var roomId2 = GenerateRoomName();
            var roomId3 = GenerateRoomName();
            var roomId4 = GenerateRoomName();
            
            _joinRoomHelper.CreateRoom("Joshua", roomId4);
            _joinRoomHelper.CreateRoom("Lydia", roomId1);
            _joinRoomHelper.CreateRoom("Andrew", roomId2);
            _joinRoomHelper.CreateRoom("Kerry", roomId3);

            var rooms = Rooms.RoomsList;
            
            rooms
                .Should()
                .ContainKey(roomId1)
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
                .ContainKey(roomId2)
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
                .ContainKey(roomId3)
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
                .ContainKey(roomId4)
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
            var roomId1 = GenerateRoomName();
            var roomId2 = GenerateRoomName();
            var roomId3 = GenerateRoomName();
            var roomId4 = GenerateRoomName();

            _joinRoomHelper.CreateRoom("Joshua", roomId1);
            _joinRoomHelper.CreateRoom("Joshua", roomId2);
            _joinRoomHelper.CreateRoom("Joshua", roomId3);
            _joinRoomHelper.CreateRoom("Joshua", roomId4);

            var rooms = Rooms.RoomsList;
            
            rooms
                .Should()
                .ContainKey(roomId1)
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
                .ContainKey(roomId2)
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
                .ContainKey(roomId3)
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
                .ContainKey(roomId4)
                .WhichValue
                .Users
                .Should()
                .ContainKey("Joshua")
                .WhichValue
                .Name
                .Should()
                .Be("Joshua");
        }
        
        private string GenerateRoomName()
        {
            var roomName = Guid.NewGuid().ToString();
            roomIds.Add(roomName);
            return roomName;
        }

        public void Dispose()
        {
            foreach (string roomId in roomIds)
            {
                Rooms.DeleteRoom(roomId);
            }
        }
    }
}