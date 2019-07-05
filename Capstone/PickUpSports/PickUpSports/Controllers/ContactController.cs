using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;
using PickUpSports.Models.ViewModel.ContactController;
using PickUpSports.Models.ViewModel.GameController;

namespace PickUpSports.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IGameService _gameService;
        private readonly IVenueService _venueService;
        private readonly IVenueOwnerService _venueOwnerService;

        public ContactController(IContactService contactService, IGameService gameService, IVenueService venueService, IVenueOwnerService venueOwnerService)
        {
            _contactService = contactService;
            _gameService = gameService;
            _venueService = venueService;
            _venueOwnerService = venueOwnerService;
        }
        
        /*
         * User's internal profile that only they have access to
         */
        public ActionResult Details()
        {
            string userEmail = User.Identity.GetUserName();

            // First check if venue owner and route to correct profile if so
            bool isVenueOwner = _venueOwnerService.IsVenueOwner(userEmail);
            if (isVenueOwner)
            {
                var owner = _venueOwnerService.GetVenueOwnerByEmail(userEmail);
                return RedirectToAction("Detail", "VenueOwner", new {id = owner.VenueOwnerId});
            }

            Contact contact = _contactService.GetContactByEmail(userEmail);
            
            return View(contact);
        }

        /*
         * Route user to page to edit their account details
         */
        public ActionResult Edit()
        {
            // Get logged in user
            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);
            if (contact == null) return HttpNotFound();

            ViewBag.States = PopulateStatesDropdown();
            
            EditContactViewModel model = new EditContactViewModel
            {
                ContactId = contact.ContactId,
                Username = contact.Username,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Address1 = contact.Address1,
                Address2 = contact.Address2,
                City = contact.City,
                State = contact.State,
                ZipCode = contact.ZipCode,
                HasPublicProfile = contact.HasPublicProfile
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditContactViewModel model)
        {
            if (ModelState.IsValid) return View(model);
            Contact existing = _contactService.GetContactById(model.ContactId);

            existing.FirstName = model.FirstName;
            existing.LastName = model.LastName;
            existing.PhoneNumber = model.PhoneNumber;
            existing.Address1 = model.Address1;
            existing.Address2 = model.Address2;
            existing.City = model.City;
            existing.State = model.State;
            existing.ZipCode = model.ZipCode;
            existing.HasPublicProfile = model.HasPublicProfile;

            _contactService.EditContact(existing);

            return RedirectToAction("Details");
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(EditContactViewModel model)
        {
            return RedirectToAction("RemoveAccount", "Account", new { id = model.ContactId});
        }

        /**
         * Endpoint that routes to public profile view. Should take in a Contact ID 
         */
        [HttpGet]
        public ActionResult PlayerProfile(int id)
        {
            var model = new ProfileViewModel();
            var contact = _contactService.GetContactById(id);
            if (contact == null) throw new ArgumentNullException($"Contact ID {id} does not exist.");

            model.Username = contact.Username;
            model.ContactId = contact.ContactId;
            model.UserAllowsPublicProfile = contact.HasPublicProfile;
            return View(model);
        }

        public ActionResult GetSportPreferences(int contactId, bool isPublicProfileView)
        {
            var model = new SportPreferenceViewModel
            {
                ContactId = contactId
            };

            model.IsPublicProfileView = isPublicProfileView;

            var results = _contactService.GetUserSportPreferences(contactId);
            if (results == null) return PartialView("../SportPreferences/_SportPreferences", model);

            model.SportName = results;
            return PartialView("../SportPreferences/_SportPreferences", model);
        }

        public ActionResult GetTimePreferences(int contactId, bool isPublicProfileView)
        {
            var model = new TimePreferenceListViewModel
            {
                ContactId = contactId,
                TimePreferences = new List<TimePreferenceViewModel>()           
            };

            model.IsPublicProfileView = isPublicProfileView;

            var timePreferences = _contactService.GetUserTimePreferences(contactId);
            if (timePreferences == null) return PartialView("../TimePreferences/_TimePreferences", model);

            foreach (var timePreference in timePreferences)
            {
                model.TimePreferences.Add(new TimePreferenceViewModel
                {
                    DayOfWeek = (DayOfWeek) timePreference.DayOfWeek,
                    BeginTime = timePreference.BeginTime,
                    EndTime = timePreference.EndTime
                });
            }

            return PartialView("../TimePreferences/_TimePreferences", model);
        }

        public ActionResult GetGamesStartedByUser(int contactId, bool isPublicProfileView)
        {
            var model = new GameProfileViewModel();
            model.IsPublicProfileView = isPublicProfileView;
            model.ContactId = contactId;

            var games = _gameService.GetCurrentOrderedGamesByContactId(contactId);
            if (games==null) return PartialView("../Game/_GamesUserCreated", model);

            model.Games = new List<GameListViewModel>();

            foreach (var game in games)
            {
                var gameToAdd = new GameListViewModel
                {
                    EndDate = game.EndTime,
                    GameId = game.GameId,
                    GameStatus = ((GameStatusEnum) game.GameStatusId).ToString(),
                    Sport = _gameService.GetSportNameById(game.SportId),
                    StartDate = game.StartTime,
                    Venue = _venueService.GetVenueNameById(game.VenueId),
                    VenueId = game.VenueId
                };

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Games.Add(gameToAdd);
            }

            return PartialView("../Game/_GamesUserCreated", model);
        }

        public ActionResult GetGamesUserJoined(int contactId, bool isPublicProfileView)
        {
            var model = new GameProfileViewModel();
            model.IsPublicProfileView = isPublicProfileView;
            model.ContactId = contactId;

            var pickUpGames = _gameService.GetPickUpGamesByContactId(contactId);
            if (pickUpGames == null) return PartialView("../Game/_GamesUserJoined", model);

            model.Games = new List<GameListViewModel>();

            foreach (var pickUpGame in pickUpGames)
            {
                var game = _gameService.GetGameById(pickUpGame.GameId);

                // Do not add game to list if time passed
                if (game.EndTime < DateTime.Today.AddDays(-1)) continue;

                // Do not add if user is the creator of game
                if (game.ContactId == contactId) continue;

                var gameToAdd = new GameListViewModel
                {
                    EndDate = game.EndTime,
                    GameId = game.GameId,
                    GameStatus = ((GameStatusEnum)game.GameStatusId).ToString(),
                    Sport = _gameService.GetSportNameById(game.SportId),
                    StartDate = game.StartTime,
                    Venue = _venueService.GetVenueNameById(game.VenueId),
                    VenueId = game.VenueId
                };

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Games.Add(gameToAdd);
            }

            model.Games = model.Games.OrderBy(x => x.StartDate).ToList();
            return PartialView("../Game/_GamesUserJoined", model);
        }

        public ActionResult GetGamesRejected(int contactId, bool isPublicProfileView)
        {
            var model = new GameProfileViewModel();
            model.IsPublicProfileView = isPublicProfileView;
            model.ContactId = contactId;

            List<Game> gameList = _gameService.GetAllGamesByContactId(model.ContactId);

            if(gameList == null) return PartialView("../Game/_GetGamesRejected", model);

            model.Games = new List<GameListViewModel>();

            foreach (var game in gameList)
            {
                // Do not add game to list if time passed
                if (game.EndTime < DateTime.Today.AddDays(-1)) continue;

                if (game.GameStatusId == 4)
                {
                    var gameToAdd = new GameListViewModel
                    {
                        EndDate = game.EndTime,
                        GameId = game.GameId,
                        GameStatus = ((GameStatusEnum)game.GameStatusId).ToString(),
                        Sport = _gameService.GetSportNameById(game.SportId),
                        StartDate = game.StartTime,
                        Venue = _venueService.GetVenueNameById(game.VenueId),
                        VenueId = game.VenueId
                    };

                    if (game.ContactId != null)
                    {
                        gameToAdd.ContactId = game.ContactId;
                        gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                    }
                    model.Games.Add(gameToAdd);
                }
            }

            model.Games = model.Games.OrderBy(x => x.StartDate).ToList();

            return PartialView("../Game/_GetGamesRejected", model);
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
