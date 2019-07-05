using System;
using System.Collections.Generic;
using System.Globalization;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.GoogleApiModels;
using System.Linq;
using PickUpSports.Interface.Repositories;

namespace PickUpSports.Services
{
    public class VenueService : IVenueService
    {
        private readonly IPlacesApiClient _placesApi;
        private readonly IVenueRepository _venueRepository;
        private readonly IBusinessHoursRepository _businessHoursRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IVenueOwnerRepository _venueOwnerRepository;

        public VenueService(IPlacesApiClient placesApi, 
            IVenueRepository venueRepository, 
            IBusinessHoursRepository businessHoursRepository, 
            IReviewRepository reviewRepository, 
            ILocationRepository locationRepository, 
            IVenueOwnerRepository venueOwnerRepository)
        {
            _placesApi = placesApi;
            _venueRepository = venueRepository;
            _businessHoursRepository = businessHoursRepository;
            _reviewRepository = reviewRepository;
            _locationRepository = locationRepository;
            _venueOwnerRepository = venueOwnerRepository;
        }

        /*
         * Return a list of all venues 
         */
        public List<Venue> GetAllVenues()
        {
            return _venueRepository.GetAllVenues();
        }

        /*
         * Given a Venue ID, return a Venue
         */
        public Venue GetVenueById(int venueId)
        {
            return _venueRepository.GetVenueById(venueId);
        }

        /*
         * Given a Venue, edit that existing Venue
         */
        public void EditVenue(Venue venue)
        {
            _venueRepository.Edit(venue);
        }

        /*
         * Given a Venue ID, return that Venue's name
         */
        public string GetVenueNameById(int venueId)
        {
            var venue = _venueRepository.GetVenueById(venueId);
            return venue.Name;
        }

        /*
         * Given a Venue ID, return that Venue's location
         */
        public Location GetVenueLocation(int venueId)
        {
            var locations = _locationRepository.GetAllLocations();
            return locations.FirstOrDefault(x => x.VenueId == venueId);
        }

        /*
         * Given a Venue ID, return all reviews for that venue
         */
        public List<Review> GetVenueReviews(int venueId)
        {
            var reviews = _reviewRepository.GetAllReviews();
            return reviews.Where(x => x.VenueId == venueId).ToList();
        }
        
        /*
         * Given a Review, add that Review to database
         */
        public void CreateVenueReview(Review review)
        {
            _reviewRepository.AddReview(review);
        }

        /*
         * Given a Review, edit that existing Review
         */
        public void EditVenueReview(Review review)
        {
            _reviewRepository.EditReview(review);
        }

        /*
         * Given a Review, delete that review from the database
         */
        public void DeleteVenueReview(Review review)
        {
            _reviewRepository.DeleteReview(review);
        }

        /*
         * Given a review ID, return that Review
         */
        public Review GetReviewById(int reviewId)
        {
           return _reviewRepository.GetReviewById(reviewId);
        }

        /*
         * Given a Venue ID, return the Business Hours for that venue
         */
        public List<BusinessHours> GetVenueBusinessHours(int venueId)
        {
            var businessHours = _businessHoursRepository.GetAllBusinessHours();
            return businessHours.Where(x => x.VenueId == venueId).ToList();
        }

        /*
         * Return a list of all business hours for all venues
         */
        public List<BusinessHours> GetAllBusinessHours()
        {
            return _businessHoursRepository.GetAllBusinessHours();
        }

        /*
         * Given a Venue, check if that Venue already has a VenueOwner
         */
        public bool VenueHasOwner(Venue venue)
        {
            //Find the owner using the venue ID, again could be simplified using repo patterns
            var allOwners = _venueOwnerRepository.GetAllVenueOwners();
            var venueOwner = allOwners.FirstOrDefault(x => x.VenueId == venue.VenueId);

            //if there is not an owner it would be null so return false
            if (venueOwner == null) return false;

            //else there is an owner and the value is not null so return true 
            return true;
        }

        /*
         * Method to check if a venue is available for a game during a certain timeframe
         */
        public bool IsVenueAvailable(List<BusinessHours> venueHours, DateTime startDateTime, DateTime endDateTime)
        {
            // If no business hours then venue has no hours and is therefore not available
            if (venueHours == null) return false;

            // Only checking start date because games should not span over a day 
            DayOfWeek startDate = startDateTime.DayOfWeek;
            BusinessHours venueOpenDate = venueHours.FirstOrDefault(x => x.DayOfWeek == (int)startDate);

            // Venue is open that date, check timeframes
            if (venueOpenDate != null)
            {
                TimeSpan startTime = startDateTime.TimeOfDay;
                TimeSpan endTime = endDateTime.TimeOfDay;

                // Change midnight to 11:59 PM for accurate time comparisons
                if (venueOpenDate.CloseTime == new TimeSpan(00, 00, 00))
                    venueOpenDate.CloseTime = new TimeSpan(23, 59, 00);

                // Ensure both start and end times are within range
                if (startTime > venueOpenDate.OpenTime && startTime < venueOpenDate.CloseTime)
                {
                    if (endTime > venueOpenDate.OpenTime && endTime < venueOpenDate.CloseTime) return true;
                }
            }

            return false;
        }

        /*
         * Method that adds and updates any venues to our database using Google Places API
         */
        public void UpdateVenues()
        {
            // Only want to update Venues database once a week
            var venues = _venueRepository.GetAllVenues();
            Venue mostRecentUpdate = venues.OrderByDescending(v => v.DateUpdated).FirstOrDefault();

            // If there is no update or if most recent update was more than two weeks ago
            // search places and update Venues database
            if (mostRecentUpdate == null || mostRecentUpdate.DateUpdated < DateTime.Now.AddDays(-14))
            {
                // Get list of places 
                List<PlaceSearchResult> places = _placesApi.GetVenues();

                foreach (var place in places)
                {
                    // Check if we already have this venue in database
                    Venue existingVenue = venues.FirstOrDefault(v => v.GooglePlaceId == place.PlaceId);

                    // If not in database, add it
                    if (existingVenue == null)
                    {
                        Venue venue = new Venue
                        {
                            GooglePlaceId = place.PlaceId,
                            Name = place.Name,
                            DateUpdated = DateTime.Now
                        };

                        _venueRepository.AddVenue(venue);
                    }
                }

                // Update most recent update
                mostRecentUpdate.DateUpdated = DateTime.Now;
                _venueRepository.Edit(mostRecentUpdate);

                UpdateVenueDetails();
            }

        }

        /*
         * Method to  calculate distance via Haversine formula.
         */
        public double CalculateVenueDistance(double lat1, double long1, double lat2, double long2)
        {
            double dDistance = Double.MinValue;
            double dLat1InRad = lat1 * (Math.PI / 180.0);
            double dLong1InRad = long1 * (Math.PI / 180);
            double dLat2InRad = lat2 * (Math.PI / 180.0);
            double dLong2InRad = long2 * (Math.PI / 180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                       Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
                       Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);
            //Intermediate result c 
            double c = 2.0 * Math.Asin(Math.Sqrt(a));

            //Distance (using approximate radius of earth in miles)
            const Double kEarthRadiusMiles = 3958.8;
            dDistance = kEarthRadiusMiles * c;
            return dDistance;
        }

        /**
         * Add or update venue details in database
         */
        private void UpdateVenueDetails()
        {
            // Get all venues 
            List<Venue> venues = _venueRepository.GetAllVenues();

            // Get only venues that don't have details
            List<Venue> venuesWithoutDetails = venues.Where(v => v.Address1 == null).ToList();

            foreach (var venue in venuesWithoutDetails)
            {

                // Venue owners have ability to manually update their venues, do not auto-update
                if (VenueHasOwner(venue)) continue;

                // Get Place details from Google API using GooglePlaceId
                PlaceDetailsResponse venueDetails = _placesApi.GetPlaceDetailsById(venue.GooglePlaceId);

                // Map response data to database model properties
                venue.Phone = venueDetails.Result.FormattedPhoneNumber;
                string streetAddress = null;
                foreach (var addressComponent in venueDetails.Result.AddressComponents)
                {
                    // Map Address from response to database model
                    var type = addressComponent.Types.FirstOrDefault();
                    switch (type)
                    {
                        case "street_number":
                            streetAddress += addressComponent.ShortName + " ";
                            break;
                        case "route":
                            streetAddress += addressComponent.ShortName;
                            break;
                        case "locality":
                            venue.City = addressComponent.ShortName;
                            break;
                        case "administrative_area_level_1":
                            venue.State = addressComponent.ShortName;
                            break;
                        case "postal_code":
                            venue.ZipCode = addressComponent.ShortName;
                            break;
                        default:
                            continue;
                    }

                    venue.Address1 = streetAddress;
                }

                // Update Venue entity
                _venueRepository.Edit(venue);

                // Map OpeningHours API response to BusinessHours entity
                if (venueDetails.Result.OpeningHours != null)
                {
                    // Initialize new BusinessHours entity using VenueID as foreign key
                    BusinessHours hours = new BusinessHours { VenueId = venue.VenueId };

                    foreach (var period in venueDetails.Result.OpeningHours.Periods)
                    {
                        hours.DayOfWeek = period.Open.Day;

                        string openTime = period.Open?.Time.Insert(2, ":");
                        if (!string.IsNullOrEmpty(openTime)) hours.OpenTime = DateTime.Parse(openTime).TimeOfDay;

                        string closeTime = period.Close?.Time.Insert(2, ":");
                        if (!string.IsNullOrEmpty(closeTime)) hours.CloseTime = DateTime.Parse(closeTime).TimeOfDay;

                        // Add BusinessHours entity
                        _businessHoursRepository.AddBusinessHours(hours);
                    }
                }

                // Map Review API response to Review database entity
                if (venueDetails.Result.Reviews != null)
                {
                    foreach (var review in venueDetails.Result.Reviews)
                    {
                        DateTime timestamp = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                        Review reviewEntity = new Review
                        {
                            IsGoogleReview = true,
                            GoogleAuthor = review.AuthorName,
                            VenueId = venue.VenueId,
                            Comments = review.Text,
                            Rating = review.Rating,

                            Timestamp = timestamp.AddSeconds(review.Time).ToLocalTime()
                        };

                        _reviewRepository.AddReview(reviewEntity);
                    }
                }

                // Map Location API response to Location database entity
                if (venueDetails.Result.Geometry != null)
                {
                    Location locationEntity = new Location
                    {
                        Latitude =
                            venueDetails.Result.Geometry.GeometryLocation.Latitude.ToString(CultureInfo.InvariantCulture),
                        Longitude =
                            venueDetails.Result.Geometry.GeometryLocation.Longitude.ToString(CultureInfo.InvariantCulture),
                        VenueId = venue.VenueId
                    };

                    // Add Location entity
                    _locationRepository.AddLocation(locationEntity);
                }
            }
        }

        /*
         * Given logged in user's email and Venue, check if the logged in user is a Venue's Owner
         */
        public bool LoggedInUserIsVenueOwner(string email, Venue venue)
        {
            VenueOwner venueOwner = _venueOwnerRepository.GetVenueOwnerByEmail(email);

            if (venueOwner == null) return false;

            if (venueOwner.VenueId != venue.VenueId) return false;

            return true;
        }

        /*
         * Given Business Hours, delete from database
         */
        public void ClearBusinessHours(List<BusinessHours> hours)
        {
            foreach (var businessHours in hours)
            {
                _businessHoursRepository.Delete(businessHours);
            }
        }

        /*
         * Given Business Hours, add to database
         */
        public void AddBusinessHour(BusinessHours hour)
        {
            _businessHoursRepository.AddBusinessHours(hour);
        }

        /*
         * Given Business Hours, update those BusinessHours in database
         */
        public void UpdateBusinessHours(BusinessHours hours)
        {
            _businessHoursRepository.Edit(hours);
        }

        /*
         * Given Business Hours, delete from database
         */
        public void DeleteBusinessHours(BusinessHours hours)
        {
           _businessHoursRepository.Delete(hours);
        }
    }
}