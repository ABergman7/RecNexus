using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Services;

namespace PickUpSportsTests
{
    [TestFixture]
    public class VenueServiceTests
    {
        private readonly Mock<IPlacesApiClient> _placesApiMock;
        private readonly Mock<IVenueRepository> _venueRepositoryMock;
        private readonly Mock<IBusinessHoursRepository> _businessHoursRepositoryMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<ILocationRepository> _locationRepositoryMock;
        private readonly Mock<IVenueOwnerRepository> _venueOwnerRepositoryMock;

        private readonly VenueService _sut;

        public VenueServiceTests()
        {
            _placesApiMock = new Mock<IPlacesApiClient>();
            _venueRepositoryMock = new Mock<IVenueRepository>();
            _businessHoursRepositoryMock = new Mock<IBusinessHoursRepository>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _locationRepositoryMock = new Mock<ILocationRepository>();
            _venueOwnerRepositoryMock = new Mock<IVenueOwnerRepository>();

            _sut = new VenueService(_placesApiMock.Object, _venueRepositoryMock.Object, _businessHoursRepositoryMock.Object, 
                _reviewRepositoryMock.Object, _locationRepositoryMock.Object, _venueOwnerRepositoryMock.Object);
        }

        /*
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenValidDateRange_ReturnsTrue()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
            { DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours
            { DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours
            { DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });

            var startDateTime = DateTime.Parse("2019-04-01 01:00 PM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, true);
        }

        /*
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenUnavailableDayOfWeek_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
            { DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours
            { DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });

            var startDateTime = DateTime.Parse("2019-04-01 01:00 PM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenNonBusinessStartTime_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
            { DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours
            { DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours
            { DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });

            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_GivenNonBusinessEndTime_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();
            venueHours.Add(new BusinessHours
            { DayOfWeek = 1, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours
            { DayOfWeek = 2, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });
            venueHours.Add(new BusinessHours
            { DayOfWeek = 3, OpenTime = TimeSpan.Parse("05:00"), CloseTime = TimeSpan.Parse("22:00") });

            var startDateTime = DateTime.Parse("2019-04-01 1:00 PM");
            var endDateTime = DateTime.Parse("2019-04-01 11:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }

        /**
         * PBI 252 - Shayna Conner
         */
        [Test]
        public void IsVenueAvailable_BusinessDoesNotHaveHours_ReturnsFalse()
        {
            // Arrange
            var venueHours = new List<BusinessHours>();

            var startDateTime = DateTime.Parse("2019-04-01 04:00 AM");
            var endDateTime = DateTime.Parse("2019-04-01 04:00 PM");

            // Act
            var isVenueAvailable = _sut.IsVenueAvailable(venueHours, startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(isVenueAvailable, false);
        }
    }
}
