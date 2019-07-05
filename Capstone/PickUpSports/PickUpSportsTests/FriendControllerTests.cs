using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PickUpSports.Controllers;
using PickUpSports.DAL;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;


namespace PickUpSportsTests
{
    [TestFixture]
    public class FriendControllerTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Mock<IGMailService> _gmailServiceMock;
        private readonly Mock<IGameService> _gamServiceMock;
        private readonly Mock<IVenueService> _venueServiceMock;

        private readonly FriendsController _sut;

        public FriendControllerTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _gmailServiceMock = new Mock<IGMailService>();
            _gamServiceMock = new Mock<IGameService>();
            _venueServiceMock = new Mock<IVenueService>();

            _sut = new FriendsController(_contactServiceMock.Object, _gmailServiceMock.Object, _gamServiceMock.Object, _venueServiceMock.Object);
        }

        [Test]
        public void IsAlreadyAFriend_IsAFriend_ReturnsTrue()
        {
            //Arrange
            Contact friend = new Contact()
                {ContactId = 2, Username = "testuser", FirstName = "Test", LastName = "Certified", Address1 = "345 Monmouth St.", Email = "testuser@gmail.com", City = "Testville", PhoneNumber = "9028472819", State = "Test", ZipCode = "902843"};

            var friendList = new List<Friend>();
            friendList.Add(new Friend
                {FriendID = 1, ContactID = 3, FriendContactID = 2});
            friendList.Add(new Friend
                { FriendID = 2, ContactID = 3, FriendContactID = 4 });
            friendList.Add(new Friend
                { FriendID = 3, ContactID = 4, FriendContactID = 5 });

            //Act
            var isAlreadyAFriend = _sut.IsAlreadyAFriend(3, friend, friendList);


            //Assert
            Assert.AreEqual(isAlreadyAFriend, true);
        }

        [Test]
        public void IsAlreadyAFriend_IsNotAFriend_ReturnsFalse()
        {
            //Arrange
            Contact friend = new Contact()
                { ContactId = 2, Username = "testuser", FirstName = "Test", LastName = "Certified", Address1 = "345 Monmouth St.", Email = "testuser@gmail.com", City = "Testville", PhoneNumber = "9028472819", State = "Test", ZipCode = "902843" };

            var friendList = new List<Friend>();
            friendList.Add(new Friend
                { FriendID = 1, ContactID = 2, FriendContactID = 3 });
            friendList.Add(new Friend
                { FriendID = 2, ContactID = 3, FriendContactID = 4 });
            friendList.Add(new Friend
                { FriendID = 3, ContactID = 4, FriendContactID = 5 });

            //Act
            var isAlreadyAFriend = _sut.IsAlreadyAFriend(3, friend, friendList);


            //Assert
            Assert.AreEqual(isAlreadyAFriend, false);
        }
    }
}
