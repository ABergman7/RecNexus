using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Services;

namespace PickUpSportsTests
{
    [TestFixture]
    public class GameServiceTests
    {
        private readonly Mock<IPickUpGameRepository> _pickUpGameRepoMock;
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<ISportRepository> _sportRepositoryMock;
        private readonly Mock<IGameStatusRepository> _gameStatusRepoMock;
        private readonly GameService _sut;

        public GameServiceTests()
        {
            _pickUpGameRepoMock = new Mock<IPickUpGameRepository>();
            _gameRepositoryMock = new Mock<IGameRepository>();
            _sportRepositoryMock = new Mock<ISportRepository>();
            _gameStatusRepoMock = new Mock<IGameStatusRepository>();
            _sut = new GameService(_pickUpGameRepoMock.Object, _gameRepositoryMock.Object, _sportRepositoryMock.Object, _gameStatusRepoMock.Object);
        }

        /**
          * PBI 267 - Shayna Conner
          */
        [Test]
        public void GetSportNameById_GivenId_ReturnsName()
        {
            // Arrange
            _sportRepositoryMock.Setup(x => x.GetSportById(It.IsAny<int>())).Returns(new Sport
            {
                SportID = 1,
                SportName = "Hello"
            });

            // Act
            var result = _sut.GetSportNameById(It.IsAny<int>());

            // Assert
            Assert.AreEqual(result, "Hello");
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetAllGamesByContactId_GivenContactId_ReturnsList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game {ContactId = 1, GameId = 1, VenueId = 1},
                new Game {ContactId = 1, GameId = 2, VenueId = 1},
                new Game {ContactId = 1, GameId = 3, VenueId = 1},
                new Game {ContactId = 2, GameId = 4, VenueId = 1},
                new Game {ContactId = 3, GameId = 5, VenueId = 1},
                new Game {ContactId = 4, GameId = 6, VenueId = 1}
            };
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);
            
            // Act
            var results = _sut.GetAllGamesByContactId(1);

            // Assert
            results.Count.Should().Be(3);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetAllGamesByContactId_IfZeroGames_ReturnNull()
        {
            // Arrange
            var games = new List<Game>();
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);

            // Act
            var results = _sut.GetAllGamesByContactId(1);

            // Assert
            Assert.IsNull(results);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetAllGamesByContactId_IfNull_ReturnNull()
        {
            // Arrange
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns((List<Game>)null);

            // Act
            var results = _sut.GetAllGamesByContactId(1);

            // Assert
            Assert.IsNull(results);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetCurrentOrderedGamesByContactId_GivenContactId_ReturnsList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game {ContactId = 1, GameId = 1, StartTime = DateTime.Today.AddDays(-4), EndTime = DateTime.Today.AddDays(-4), VenueId = 1},
                new Game {ContactId = 1, GameId = 2, StartTime = DateTime.Today.AddDays(3), EndTime = DateTime.Today.AddDays(3), VenueId = 1},
                new Game {ContactId = 1, GameId = 3, StartTime = DateTime.Today.AddDays(6), EndTime = DateTime.Today.AddDays(6), VenueId = 1},
                new Game {ContactId = 2, GameId = 4, VenueId = 1},
                new Game {ContactId = 3, GameId = 5, VenueId = 1},
                new Game {ContactId = 4, GameId = 6, VenueId = 1},
                new Game {ContactId = 1, GameId = 7, StartTime = DateTime.Today.AddDays(-2), EndTime = DateTime.Today.AddDays(-2), VenueId = 1},

            };
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);

            // Act
            var results = _sut.GetCurrentOrderedGamesByContactId(1);

            // Assert
            results.Count.Should().Be(2);
        }

        /**
          * PBI 327 - Shayna Conner
          */
        [Test]
        public void GetCurrentOrderedGamesByContactId_IfNoGames_ReturnsNull()
        {
            // Arrange
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns((List<Game>)null);

            // Act
            var results = _sut.GetCurrentOrderedGamesByContactId(1);

            // Assert
            Assert.IsNull(results);
        }

        /**
          * PBI 324 - Kexin Pan
          */
        [Test]
        public void GetCurrentGamesByVenueId_GivenVenueId_ReturnsList()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game {ContactId = 2, GameId = 4, VenueId = 23, EndTime = DateTime.Now},
                new Game {ContactId = 3, GameId = 5, VenueId = 22, EndTime = DateTime.Now},
                new Game {ContactId = 4, GameId = 6, VenueId = 23, EndTime = DateTime.Now},
                new Game {ContactId = 1, GameId = 9, VenueId = 23, EndTime = DateTime.Now},

            };
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns(games);

            // Act
            var results = _sut.GetCurrentGamesByVenueId(23);

            // Assert
            results.Count.Should().Be(3);
        }

        /**
          * PBI 324 - Kexin Pan
          */
        [Test]
        public void GetCurrentGamesByVenueId_GivenVenueId_ReturnsNull()
        {
            // Arrange
            _gameRepositoryMock.Setup(x => x.GetAllGames()).Returns((List<Game>)null);

            // Act
            var results = _sut.GetCurrentGamesByVenueId(30);

            // Assert
            Assert.IsNull(results);
        }

        /**
 * PBI 142 - Shion Wakita
 */
        [Test]
        public void IsCreatorOfGame_UserIsNotCreator_ReturnsFalse()
        {
            //Arrange 
            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            Game gameTest1 = new Game() { ContactId = 1, VenueId = 2, GameId = 3, GameStatusId = 1, SportId = 1, StartTime = startDateTime, EndTime = endDateTime };
            Game gameTest2 = new Game() { ContactId = 2, VenueId = 2, GameId = 3, GameStatusId = 1, SportId = 1, StartTime = startDateTime, EndTime = endDateTime };

            //Act
            var isCreatorOfGameTest1 = _sut.IsCreatorOfGame(2, gameTest1);
            var isCreatorOfGameTest2 = _sut.IsCreatorOfGame(1, gameTest2);

            //Assert
            Assert.AreEqual(isCreatorOfGameTest1, false);
            Assert.AreEqual(isCreatorOfGameTest2, false);
        }

        /**
         * PBI 142 - Shion Wakita
         */
        [Test]
        public void IsCreatorOfGame_UserIsCreator_ReturnsTrue()
        {
            //Arrange 
            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            Game gameTest1 = new Game() { ContactId = 1, VenueId = 3, GameId = 4, GameStatusId = 1, SportId = 2, StartTime = startDateTime, EndTime = endDateTime };
            Game gameTest2 = new Game() { ContactId = 2, VenueId = 4, GameId = 3, GameStatusId = 1, SportId = 1, StartTime = startDateTime, EndTime = endDateTime };

            //Act
            var isCreatorOfGameTest1 = _sut.IsCreatorOfGame(1, gameTest1);
            var isCreatorOfGameTest2 = _sut.IsCreatorOfGame(2, gameTest2);

            //Assert
            Assert.AreEqual(isCreatorOfGameTest1, true);
            Assert.AreEqual(isCreatorOfGameTest2, true);

        }

        // Kexin Pan
        [Test]
        public void IsSelectedTimeValid_TimeIsValid_ReturnsTrue()
        {
            var startDateTime = DateTime.Parse("2019-04-11 09:00 PM");
            var endDateTime = DateTime.Parse("2019-04-11 10:00 PM");

            var isSelectedTimeValideTest = _sut.IsSelectedTimeValid(startDateTime, endDateTime);
            Assert.AreEqual(isSelectedTimeValideTest, true);
        }

        [Test]
        public void IsSelectedTimeValid_TimeIsNotValid_ReturnsFalse()
        {
            var startDateTime = DateTime.Parse("2019-04-12 09:00 PM");
            var endDateTime = DateTime.Parse("2019-04-22 10:00 PM");

            var isSelectedTimeValideTest = _sut.IsSelectedTimeValid(startDateTime, endDateTime);
            Assert.AreEqual(isSelectedTimeValideTest, false);
        }


        /**
         * PBI 254 - Shion Wakita
         */
        [Test]
        public void IsNotSignedUpForGame_UserIsNotSignedUp_ReturnsTrue()
        {
            //Arrange
            List<PickUpGame> playerList = new List<PickUpGame>();
            playerList.Add(new PickUpGame()
            { ContactId = 1, GameId = 2, PickUpGameId = 1 });
            playerList.Add(new PickUpGame()
            { ContactId = 2, GameId = 2, PickUpGameId = 2 });

            //Act
            var isNotSignedUpForGameTest = _sut.IsNotSignedUpForGame(3, playerList);

            //Assert
            Assert.AreEqual(isNotSignedUpForGameTest, true);

        }

        [Test]
        public void IsNotSignedUpForGame_UserIsSignedUp_ReturnsFalse()
        {
            //Arrange
            List<PickUpGame> playerList = new List<PickUpGame>();
            playerList.Add(new PickUpGame()
            { ContactId = 1, GameId = 2, PickUpGameId = 1 });
            playerList.Add(new PickUpGame()
            { ContactId = 2, GameId = 2, PickUpGameId = 2 });

            //Act
            var isNotSignedUpForGameTest = _sut.IsNotSignedUpForGame(1, playerList);

            //Assert
            Assert.AreEqual(isNotSignedUpForGameTest, false);
        }

        /**
         * PBI 143 - Kexin Pan
         */
        [Test]
        public void IsThisGameCanCancel_AtLeastBeforeOneHour_ReturnsTrue()
        {
            var startTime = DateTime.Parse("2020-05-15 7:00 PM");
            Assert.AreEqual(_sut.IsThisGameCanCancel(startTime), true);
        }

        [Test]
        public void IsThisGameCanCancel_NotBeforeOneHour_ReturnsFalse()
        {
            var startTime = DateTime.Parse("2019-04-12 09:00 PM");
            Assert.AreEqual(_sut.IsThisGameCanCancel(startTime), false);
        }

    }

}
