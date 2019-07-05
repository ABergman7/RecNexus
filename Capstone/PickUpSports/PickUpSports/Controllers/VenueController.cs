using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;
using PickUpSports.Models.Extensions;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.GameController;
using PickUpSports.Models.ViewModel.VenueController;
using PickUpSports.Models.ViewModel.VenueOwnerController;

namespace PickUpSports.Controllers
{
    public class VenueController : Controller
    {
        private readonly IVenueService _venueService;
        private readonly IContactService _contactService;
        private readonly IGameService _gameService;
        private readonly IVenueOwnerService _venueOwnerService;

        public VenueController(IVenueService venueService, 
            IContactService contactService, 
            IGameService gameService,
            IVenueOwnerService venueOwnerService)
        {
            _venueService = venueService;
            _contactService = contactService;
            _gameService = gameService;
            _venueOwnerService = venueOwnerService;
        }
        
        /*
         * Show a list of venues
         */
        public ActionResult Index()
        {
            _venueService.UpdateVenues();

            // Create view model for list of venues
            SearchVenueViewModel model = new SearchVenueViewModel();
            model.Venues = new List<VenueViewModel>();
            List<Venue> venues = _venueService.GetAllVenues();

            foreach (var venue in venues)
            {
                // get location of venue
                Location location = _venueService.GetVenueLocation(venue.VenueId);
                
                // get the rating 
                List<Review> reviews = _venueService.GetVenueReviews(venue.VenueId);

                decimal? avgRating;
                if (reviews.Count > 0)
                {
                    avgRating = (decimal) reviews.Average(r => r.Rating);
                }
                else avgRating = null;


                model.Venues.Add(new VenueViewModel
                {
                    Address1 = venue.Address1,
                    Address2 = venue.Address2,
                    City = venue.City,
                    Name = venue.Name,
                    Phone = venue.Phone,
                    State = venue.State,
                    VenueId = venue.VenueId,
                    ZipCode = venue.ZipCode,
                    AverageRating = avgRating,
                    LatitudeCoord = location.Latitude,
                    LongitudeCoord = location.Longitude,
                });
            }

            return View(model);
        }

        /*
         * Venue search and filter results
         */
        [HttpPost]
        public ActionResult Filter(SearchVenueViewModel model)
        {
            model.Venues = (List<VenueViewModel>) TempData["Venues"];

            // Create new model to send to view
            SearchVenueViewModel viewModel = new SearchVenueViewModel();
            viewModel.Venues = new List<VenueViewModel>();

            if (model.Venues.Count == 0)
            {
                ViewBag.Error = "No results to filter. Please click \"Reset all filters\"";
                return View("Index", viewModel);
            }

            // User entered a search string
            if (!string.IsNullOrEmpty(model.Search))
            {
                // Search venue names and cities
                viewModel.Venues.AddRange(model.Venues.Where(x => x.Name.CaseInsensitiveContains(model.Search)).ToList());
                viewModel.Venues.AddRange(model.Venues.Where(x => x.City.CaseInsensitiveContains(model.Search)));
                
                // Get first venue on list and assign coordinates to map center
                var firstVenue = viewModel.Venues.FirstOrDefault();
                if (firstVenue != null)
                {
                    viewModel.CurrentLatitude = firstVenue.LatitudeCoord;
                    viewModel.CurrentLongitude = firstVenue.LongitudeCoord;
                }
            }

            if (!string.IsNullOrEmpty(model.Filter))
            {
                // User chose to sort by rating
                if (model.Filter.Equals("Rating"))
                {
                    viewModel.Venues = model.Venues.OrderByDescending(x => x.AverageRating).ToList();

                    if (viewModel.Venues.Count == 0)
                    {
                        ViewBag.Error = "No search results matching given filters";
                    }

                    // Get first venue on list and assign coordinates to map center
                    var firstVenue = viewModel.Venues.FirstOrDefault();
                    if (firstVenue != null)
                    {
                        viewModel.CurrentLatitude = firstVenue.LatitudeCoord;
                        viewModel.CurrentLongitude = firstVenue.LongitudeCoord;
                    }
                }
                // User chose to filter by time
                if (model.Filter.Equals("Time"))
                {
                    //get all the buiness hours for all the venues
                    List<BusinessHours> hours = _venueService.GetAllBusinessHours();

                    //lets wait till time is NOT null
                    if (model.Time != null)
                    {
                        //convert user input string to a TimeSpan
                        TimeSpan hs = TimeSpan.Parse(model.Time);

                        var dayOfWeek = Enum.Parse(typeof(DayOfWeek), model.Day);
                        //First filter out using the days
                        List<BusinessHours> day_available = hours.Where(x => x.DayOfWeek == (int) dayOfWeek).ToList();

                        //Then use that list and filter the closed times
                        List<BusinessHours> closed_from = day_available.Where(x => x.CloseTime >= hs).ToList();

                        //Finally filter the list above and you have the remaning few which maches the days and the closing time
                        List<BusinessHours> open_from = closed_from.Where(x => x.OpenTime <= hs).ToList();

                        //If there are some in the list that matches the user input - else it didnt match
                        if (open_from.Count > 0)
                        {
                            foreach (var businessHour in open_from)
                            {
                                // Only keep venues that are in open_from
                                var venue = model.Venues.FirstOrDefault(x => x.VenueId == businessHour.VenueId);
                                if (venue != null && !viewModel.Venues.Contains(venue)) viewModel.Venues.Add(venue);
                            }
                        }
                    }

                    // Get first venue on list and assign coordinates to map center
                    var firstVenue = viewModel.Venues.FirstOrDefault();
                    if (firstVenue != null)
                    {
                        viewModel.CurrentLatitude = firstVenue.LatitudeCoord;
                        viewModel.CurrentLongitude = firstVenue.LongitudeCoord;
                    }
                }

                // User chose to filter by distance
                if (model.Filter.Equals("Distance"))
                {
                    foreach (var venue in model.Venues)
                    {
                        //get location of venue
                        Location location = _venueService.GetVenueLocation(venue.VenueId);

                        //converted coordinates from strings to doubles
                        double userLat = Convert.ToDouble(model.CurrentLatitude);
                        double userLong = Convert.ToDouble(model.CurrentLongitude);
                        double venLat = Convert.ToDouble(location.Latitude);
                        double venLong = Convert.ToDouble(location.Longitude);

                        //Calculate the distance from user to venue. Method at the bottom
                        venue.Distance = _venueService.CalculateVenueDistance(userLat, userLong, venLat, venLong);
                        viewModel.Venues.Add(venue);
                    }

                    viewModel.Venues = viewModel.Venues.OrderBy(x => x.Distance).ToList();

                    // Get closest venue to user and assign coordinates to map center
                    var closest = viewModel.Venues.First();
                    viewModel.CurrentLatitude = closest.LatitudeCoord;
                    viewModel.CurrentLongitude = closest.LongitudeCoord;

                }
            }
           
            if (viewModel.Venues.Count == 0)
            {
                ViewBag.Error = "No search results matching given filters";
            }

            // Preserve search/filter values
            viewModel.Day = model.Day;
            viewModel.Time = model.Time;
            viewModel.Search = model.Search;

            if (string.IsNullOrEmpty(viewModel.CurrentLatitude))
            {
                viewModel.CurrentLatitude = model.CurrentLatitude;
                viewModel.CurrentLongitude = model.CurrentLongitude;
            }

            model.Venues.Clear();
            return View("Index", viewModel);
        }

        /*
         * Get venue, hours, and review data for single Venue and return to view
         */
        public ActionResult Details(int id)
        {
            // Initializing Owner authentication variable 
            ViewBag.IsOwner = false;

            // Model to be sent to view
            VenueViewModel model = new VenueViewModel();

            // Map venue details
            Venue venue = _venueService.GetVenueById(id);
            model.Address1 = venue.Address1;
            model.Address2 = venue.Address2;
            model.City = venue.City;
            model.Name = venue.Name;
            model.Phone = venue.Phone;
            model.State = venue.State;
            model.VenueId = venue.VenueId;
            model.ZipCode = venue.ZipCode;

            //Checking if the user is logged in & and is the owner
            bool loggedInUser = User.Identity.IsAuthenticated;

            if (loggedInUser)
            {
                string email = User.Identity.GetUserName();
                bool isOwner = _venueService.LoggedInUserIsVenueOwner(email, venue);

                // If logged-in user is the owner, then show link to edit venue 
                if (isOwner)
                {
                    ViewBag.IsOwner = true;
                }
            }

            // Check if venue owner exists
            model.HasVenueOwner = _venueService.VenueHasOwner(venue);

            if (model.HasVenueOwner)
            {
                model.VenueOwner = new VenueOwnerViewModel();

                var venueOwner = _venueOwnerService.GetVenueOwnerByVenueId(id);
                model.VenueOwner.FirstName = venueOwner.FirstName;
                model.VenueOwner.LastName = venueOwner.LastName;
                model.VenueOwner.VenueOwnerId = venueOwner.VenueOwnerId;
            }

            // Map business hours
            List<BusinessHours> businessHours = _venueService.GetVenueBusinessHours(id);
            model.BusinessHours = new List<BusinessHoursViewModel>();

            foreach (var businessHour in businessHours)
            {
                var closeDateTime = new DateTime() + businessHour.CloseTime;
                var openDateTime = new DateTime() + businessHour.OpenTime;

                model.BusinessHours.Add(new BusinessHoursViewModel
                {
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), businessHour.DayOfWeek),
                    CloseTime = closeDateTime.ToShortTimeString(),
                    OpenTime = openDateTime.ToShortTimeString()
                });
            }

            // Map reviews 
            List<Review> reviews = _venueService.GetVenueReviews(id);

            decimal? avgRating;
            if (reviews.Count > 0)
            {
                avgRating = (decimal) reviews.Average(r => r.Rating);
                model.AverageRating = Math.Round((decimal) avgRating, 1);
            }
            else model.AverageRating = null;         

            return View(model);
        }

        public PartialViewResult GetReviews(int venueId)
        {
            var reviews = _venueService.GetVenueReviews(venueId);
            var model = new PartialReviewViewModel{VenueId = venueId};

            if (reviews.Count > 0)
            {
                var averageRating = (decimal)reviews.Average(r => r.Rating);
                model.AverageReviewRating = Math.Round((decimal)averageRating, 1);
            }
            else model.AverageReviewRating = null;

            List<ReviewViewModel> tempList = new List<ReviewViewModel>();
            foreach (var review in reviews)
            {
                ReviewViewModel reviewModel = new ReviewViewModel
                {
                    Rating = review.Rating,
                    Timestamp = review.Timestamp,
                    Comments = review.Comments,
                    ReviewId = review.ReviewId
                };

                // If review is not a Google review then there is no GoogleAuthor, 
                // find author in Contact table
                if (review.IsGoogleReview) reviewModel.Author = review.GoogleAuthor;
                else
                {
                    Contact user = _contactService.GetContactById(review.ContactId);
                    if (user == null) reviewModel.Author = null;
                    else
                    {
                        reviewModel.Author = user.Username;

                        // Check if the logged in user wrote this review
                        string loggedInUserEmail = User.Identity.GetUserName();

                        if (user.Email == loggedInUserEmail)
                        {
                            reviewModel.ReviewBelongsToUser = true;
                        }
                        else reviewModel.ReviewBelongsToUser = false;
                    }
                }

                tempList.Add(reviewModel);
            }

            // Order reviews newest to oldest
            model.Reviews = tempList.OrderByDescending(r => r.Timestamp).ToList();
            return PartialView("_VenueReviews", model);
        }

        public PartialViewResult GetVenueGames(int venueId)
        {
            var model = new List<GameListViewModel>();
            var games = _gameService.GetCurrentGamesByVenueId(venueId);
            if (games == null) return PartialView("_VenueGames", model);

            var openGames = games.Where(x => x.GameStatusId == (int) GameStatusEnum.Open).ToList();
            if (openGames.Count < 1) return PartialView("_VenueGames", model);

            foreach (var game in games)
            {
                GameListViewModel gameToAdd = new GameListViewModel
                {
                    GameId = game.GameId,
                    Sport = _gameService.GetSportNameById(game.SportId),
                    StartDate = game.StartTime,
                    EndDate = game.EndTime
                };

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Add(gameToAdd);
            }

            model = new List<GameListViewModel>(model.OrderBy(x => x.StartDate));
            return PartialView("_VenueGames", model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditVenue(int id)
        {
            //Populate states dropdown
            ViewBag.States = PopulateStatesDropdown();

            //Get the current logged in user
            string currUser = User.Identity.GetUserName();

            //If the current user is not the venue owner then redirect them to details of venue
            if (!_venueOwnerService.IsVenueOwner(currUser))
            {
                ViewBag.States = PopulateStatesDropdown();
                return RedirectToAction("Details", new { id = id });
            }

            //Find the venues with the ID
            Venue venue = _venueService.GetVenueById(id);

            //Gotta get business hours for the venues 
            List<BusinessHours> businessHours = _venueService.GetVenueBusinessHours(id);

            //create a shell for business hours with complete set of day of weeks
            List<BusinessHours> shell = new List<BusinessHours>();
            for (int i = 0; i < 7; i++)
            {
                shell.Add(new BusinessHours()
                {
                    DayOfWeek = i,
                    VenueId = id
                });
            }

            //If there is an existing hours for the venue, match it with the day of week in the shell and insert it 
            foreach (var hours in businessHours)
            {
                BusinessHours matchHours = shell.Find(x => x.DayOfWeek == hours.DayOfWeek);
                matchHours.BusinessHoursId = hours.BusinessHoursId;
                matchHours.CloseTime = hours.CloseTime;
                matchHours.OpenTime = hours.OpenTime;
            }

            //Creating a Business hour VM for the view model passing back to the venue view
            List<BusinessHoursViewModel> businessHoursViewModels = new List<BusinessHoursViewModel>();

            foreach (var hours in shell)
            {
                //initializing the opening and closing string 
                string openTimeString = "";
                string closingTimeString = "";

                //if the hours are not null then pass it in
                if (hours.CloseTime != TimeSpan.Zero || hours.OpenTime != TimeSpan.Zero)
                {
                    openTimeString = hours.OpenTime.ToString();
                    closingTimeString = hours.CloseTime.ToString();
                }

                //Create a new Business hours view model with it, either if it has a data or an empty string
                BusinessHoursViewModel hoursViewModel = new BusinessHoursViewModel()
                { 
                    DayOfWeek = Enum.Parse(typeof(DayOfWeek), hours.DayOfWeek.ToString()).ToString(),
                    CloseTime = closingTimeString,
                    OpenTime = openTimeString
                };

                //Add the business hours
                businessHoursViewModels.Add(hoursViewModel);
            }

            //Create the View Model for the Edit View
            VenueViewModel model = new VenueViewModel
            {
                VenueId = venue.VenueId,
                Name = venue.Name,
                Address1 = venue.Address1,
                Address2 = venue.Address2,
                City = venue.City,
                Phone = venue.Phone,
                State = venue.State,
                ZipCode = venue.ZipCode,
                BusinessHours = businessHoursViewModels
            };


            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditVenue(VenueViewModel model)
        {
            //Populate states dropdown
            ViewBag.States = PopulateStatesDropdown();

            //Editing the venue details 
            Venue existingVenue = _venueService.GetVenueById(model.VenueId);

            existingVenue.Address1 = model.Address1;
            existingVenue.Address2 = model.Address2;
            existingVenue.City = model.City;
            existingVenue.DateUpdated = DateTime.Now;
            existingVenue.Name = model.Name;
            existingVenue.Phone = model.Phone;
            existingVenue.State = model.State;
            existingVenue.ZipCode = model.ZipCode;

            //find the current hours of the venue using the venueId
            List<BusinessHours> existingHours = _venueService.GetVenueBusinessHours(model.VenueId);

            TimeSpan beginTimeSpan = TimeSpan.Zero;
            TimeSpan endTimeSpan = TimeSpan.Zero;

            //for each hours in the view modeling hours - runs 7 times since the days of weeks are not null
            foreach (var hours in model.BusinessHours)
            {


                //convert the string day of week to numbers to be able to pass it back into the dB
                DayOfWeek convertDayOfWeek = (DayOfWeek) (Enum.Parse(typeof(DayOfWeek), hours.DayOfWeek));

                //Match the day of week with the existing 
                BusinessHours checkHours = existingHours.Find(x => x.DayOfWeek == (int)convertDayOfWeek);

                //Check if only one field is filled and the other isn't
                if (hours.OpenTime != null && hours.CloseTime == null)
                { 
                    ViewBag.States = PopulateStatesDropdown();
                    ViewData.ModelState.AddModelError("BothTimeFilled", "Please fill out both Opening and Ending Time for the certain day");
                    return View(model);
                }
                if (hours.OpenTime == null && hours.CloseTime != null)
                {
                    ViewBag.States = PopulateStatesDropdown();
                    ViewData.ModelState.AddModelError("BothTimeFilled", "Please fill out both Opening and Ending Time for the certain day");
                    return View(model);
                }

                //If the Open Time and the Closing Time are not null, there was time entered into it
                if (hours.OpenTime != null && hours.CloseTime != null)
                {
                    //Checking if it is a valid format, if so assign it to the TimeSpan variable initialized above 
                    bool checkOpenTime = TimeSpan.TryParse(hours.OpenTime, out beginTimeSpan);
                    bool checkCloseTime = TimeSpan.TryParse(hours.CloseTime, out endTimeSpan);

                    if(!checkCloseTime && !checkOpenTime)
                    {
                        ViewBag.States = PopulateStatesDropdown();
                        ViewData.ModelState.AddModelError("ValidTime", "The time must be between 00:00 - 23:00");
                        return View(model);
                    }


                    //checking if opening time and closing time is valid
                    if (beginTimeSpan >= endTimeSpan)
                    {
                        ViewBag.States = PopulateStatesDropdown();
                        ViewData.ModelState.AddModelError("TimeRange", "The Opening Time cannot be after or equal to the Closing Time");
                        return View(model);
                    }

                    //Case: 1: Adding new Business Hours - If checkHours equals null, then there was no match so time to add in a new business hour
                    if (checkHours == null)
                    {
                        _venueService.AddBusinessHour(new BusinessHours()
                        {
                            VenueId = model.VenueId,
                            CloseTime = TimeSpan.Parse(hours.CloseTime),
                            OpenTime = TimeSpan.Parse(hours.OpenTime),
                            DayOfWeek = (int) convertDayOfWeek
                        });

                    }
                    //Case: 2 Modify Business Hours - if checkHours is not null there was a match 
                    else
                    {
                        checkHours.CloseTime = TimeSpan.Parse(hours.CloseTime);
                        checkHours.OpenTime = TimeSpan.Parse(hours.OpenTime);

                        _venueService.UpdateBusinessHours(checkHours);
                    }

                }

                //Case:3 Deleting Business Hours - user removes the business hours from the venue
                if (checkHours != null && (hours.OpenTime == null && hours.CloseTime == null))
                {
                    _venueService.DeleteBusinessHours(checkHours);
                }
            }

            //Edit the generic information of the venue excluding the business hours
            _venueService.EditVenue(existingVenue);



            return RedirectToAction("Details", new { id = model.VenueId });
        }

        private Dictionary<string, string> PopulateStatesDropdown()
        {
            var states = new Dictionary<string, string>();

            states.Add("AL", "Alabama");
            states.Add("AK", "Alaska");
            states.Add("AZ", "Arizona");
            states.Add("AR", "Arkansas");
            states.Add("CA", "California");
            states.Add("CO", "Colorado");
            states.Add("CT", "Connecticut");
            states.Add("DE", "Delaware");
            states.Add("DC", "District of Columbia");
            states.Add("FL", "Florida");
            states.Add("GA", "Georgia");
            states.Add("HI", "Hawaii");
            states.Add("ID", "Idaho");
            states.Add("IL", "Illinois");
            states.Add("IN", "Indiana");
            states.Add("IA", "Iowa");
            states.Add("KS", "Kansas");
            states.Add("KY", "Kentucky");
            states.Add("LA", "Louisiana");
            states.Add("ME", "Maine");
            states.Add("MD", "Maryland");
            states.Add("MA", "Massachusetts");
            states.Add("MI", "Michigan");
            states.Add("MN", "Minnesota");
            states.Add("MS", "Mississippi");
            states.Add("MO", "Missouri");
            states.Add("MT", "Montana");
            states.Add("NE", "Nebraska");
            states.Add("NV", "Nevada");
            states.Add("NH", "New Hampshire");
            states.Add("NJ", "New Jersey");
            states.Add("NM", "New Mexico");
            states.Add("NY", "New York");
            states.Add("NC", "North Carolina");
            states.Add("ND", "North Dakota");
            states.Add("OH", "Ohio");
            states.Add("OK", "Oklahoma");
            states.Add("OR", "Oregon");
            states.Add("PA", "Pennsylvania");
            states.Add("RI", "Rhode Island");
            states.Add("SC", "South Carolina");
            states.Add("SD", "South Dakota");
            states.Add("TN", "Tennessee");
            states.Add("TX", "Texas");
            states.Add("UT", "Utah");
            states.Add("VT", "Vermont");
            states.Add("VA", "Virginia");
            states.Add("WA", "Washington");
            states.Add("WV", "West Virginia");
            states.Add("WI", "Wisconsin");
            states.Add("WY", "Wyoming");

            return states;
        }
    }
}