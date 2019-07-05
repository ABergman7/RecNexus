using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;
using PickUpSports.Models.ViewModel;
using PickUpSports.Models.ViewModel.GameController;
using DayOfWeek = System.DayOfWeek;

namespace PickUpSports.Controllers
{
    public class GameController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IGMailService _gMailer;
        private readonly IGameService _gameService;
        private readonly IVenueService _venueService;
        private readonly IVenueOwnerService _venueOwnerService;

        public GameController(IContactService contactService, 
            IGMailService gMailer, 
            IGameService gameService, 
            IVenueService venueService,
            IVenueOwnerService venueOwnerService)
        {
            _contactService = contactService;
            _gMailer = gMailer;
            _gameService = gameService;
            _venueService = venueService;
            _venueOwnerService = venueOwnerService;
        }

        /**
         * Routes user to page that contains Create Game form
         */
        [Authorize]
        public ActionResult CreateGame()
        {
            ViewBag.GameCreated = false;
            PopulateDropdownValues();
            return View();
        }

        /**
         * Handles input from Create Game form 
         */
        [HttpPost]
        public ActionResult CreateGame(CreateGameViewModel model)
        {
            // Check model validation before doing anything
            if (!ModelState.IsValid)
            {
                PopulateDropdownValues();
                return View(model);
            }

            // Get start and end dates
            var dates = model.DateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.InvariantCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            // Return error to View if user picks different dates for start and end
            if (startDateTime.DayOfWeek != endDateTime.DayOfWeek)
            {
                ViewData.ModelState.AddModelError("DateRange", "Start date and end date must be same date.");
                PopulateDropdownValues();
                return View(model);
            }

            // Check if similar game already exists
            Game existingGame = _gameService.CheckForExistingGame(model.VenueId, model.SportId, startDateTime);
            if (existingGame != null)
            {
                ViewData.ModelState.AddModelError("GameExists", "A game already exists with this venue, sport, and time.");
                PopulateDropdownValues();
                return View(model);
            }

            // Get venue by ID and business hours for that venue
            Venue venue = _venueService.GetVenueById(model.VenueId);
            List<BusinessHours> venueHours = _venueService.GetVenueBusinessHours(model.VenueId);

            // Return error to View if the venue is not available
            bool isVenueAvailable = _venueService.IsVenueAvailable(venueHours, startDateTime, endDateTime);
            if (!isVenueAvailable)
            {
                ViewData.ModelState.AddModelError("DateRange", $"Unfortunately, {venue.Name} is not available during the hours you chose.");
                PopulateDropdownValues();
                return View(model);
            }

            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            // All validation passed so add game to database 
            Game newGame = new Game
            {
                ContactId = contact.ContactId,
                GameStatusId = (int)GameStatusEnum.Open,
                VenueId = model.VenueId,
                SportId = model.SportId,
                StartTime = startDateTime,
                EndTime = endDateTime
            };

            _gameService.CreateGame(newGame);

            ViewBag.GameCreated = true;
            PopulateDropdownValues();

            //email content
            var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailFormat.html"));
            //add game link to the email
            var directUrl = Url.Action("GameDetails", "Game", new { id = newGame.GameId }, protocol: Request.Url.Scheme);
            fileContents = fileContents.Replace("{URL}", directUrl);
            //replace the html contents to the game details           
            fileContents = fileContents.Replace("{VENUE}", venue.Name);
            fileContents = fileContents.Replace("{SPORT}", _gameService.GetSportNameById(model.SportId));
            fileContents = fileContents.Replace("{STARTTIME}", startDateTime.ToString());
            fileContents = fileContents.Replace("{ENDTIME}", endDateTime.ToString());
            

            //send notification to users once a new game created and it includes the user's preference
            List<SportPreference> checkSportPreference = _contactService.GetAllSportPreferences();
            
            foreach (var item in checkSportPreference)
            {
                if (item.SportID == model.SportId && item.ContactID != newGame.ContactId)
                {
                    fileContents = fileContents.Replace("{URLNAME}", "JOIN");
                    fileContents = fileContents.Replace("{TITLE}", "New Games Coming!!!");
                    fileContents = fileContents.Replace("{INFO}", "We have a new game you may interested!");
                    var subject = "New Game At Rec Nexus";
                    SendMessage(newGame, item.ContactID, fileContents, subject);
                    
                }
            }

            // time preference
            List<TimePreference> checkTimePreferences = _contactService.GetAllTimePreferences();
            List<Contact> nonDuplicateUser = new List<Contact>();
            bool duplicate = false ;
            foreach (var item in checkTimePreferences)
            {
                if ((int)newGame.StartTime.DayOfWeek==item.DayOfWeek
                    && newGame.StartTime.TimeOfDay>item.BeginTime && newGame.EndTime.TimeOfDay<item.EndTime
                    && item.ContactID != newGame.ContactId)
                {
                    foreach(var user in nonDuplicateUser)
                    {
                        if (user.ContactId == item.ContactID)
                        {
                            duplicate = true;
                        }
                        else
                        {
                            duplicate = false;
                        }
                    }
                    if (!duplicate)
                    {
                        nonDuplicateUser.Add(_contactService.GetContactById(item.ContactID));
                    }
                    foreach (var checkeduser in nonDuplicateUser)
                    {
                        fileContents = fileContents.Replace("{URLNAME}", "JOIN");
                        fileContents = fileContents.Replace("{TITLE}", "New Games Coming!!!");
                        fileContents = fileContents.Replace("{INFO}", "We have a new game you may interested!");
                        var subject = "New Game At Rec Nexus";
                        SendMessage(newGame, item.ContactID, fileContents, subject);
                    }
                }
            }

            //send notification to the venue owner
            if (_venueService.VenueHasOwner(venue))
            {
                int ownerId= _venueOwnerService.GetVenueOwnerByVenueId(venue.VenueId).VenueOwnerId;
                fileContents = fileContents.Replace("{URLNAME}", "CHECK");
                fileContents = fileContents.Replace("{TITLE}", "New Game Created on Your Venue!");
                fileContents = fileContents.Replace("{INFO}", "There is a new game created on your venue, please check and make sure there is no conflict with the venue schedule.");
                var subject = "New Game on Your Venue";
                SendMessage(newGame, ownerId, fileContents, subject);
            }

            return View();
        }

        /**
         * Routes user to page that shows a list of all current, open games
         */
        public ActionResult GameList()
        {
            ViewBag.Venue = new SelectList(_venueService.GetAllVenues(), "VenueId", "Name");
            ViewBag.Sport = new SelectList(_gameService.GetAllSports(), "SportId", "SportName");

            // Get games that are open and that have not already passed and order by games happening soonest
            List<Game> games = _gameService.GetAllCurrentOpenGames();

            List<GameListViewModel> model = new List<GameListViewModel>();
            foreach (var game in games)
            {
                var gameToAdd = new GameListViewModel();
                gameToAdd.GameId = game.GameId;
                gameToAdd.Sport = _gameService.GetSportNameById(game.SportId);
                gameToAdd.Venue = _venueService.GetVenueNameById(game.VenueId);
                gameToAdd.StartDate = game.StartTime;
                gameToAdd.EndDate = game.EndTime;

                if (game.ContactId != null)
                {
                    gameToAdd.ContactId = game.ContactId;
                    gameToAdd.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                model.Add(gameToAdd);
            }

            return View(model);
        }

        /**
         * Routes user to GameDetails page to show details for single game
         */
        [Authorize]
        [HttpGet]
        public ActionResult GameDetails(int id)
        {
            //validating the id to make sure its not null
            if (id == 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewGameViewModel model = new ViewGameViewModel();

            //getting current logged in user information in case they want to join game
            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            Game game = _gameService.GetGameById(id);

            if (contact == null)
            {
                // Check if is venue owner. If not, they did not initalize their profile 
                VenueOwner venueOwner = _venueOwnerService.GetVenueOwnerByEmail(email);
                model.IsCreatorOfGame = false;
                if (venueOwner.VenueId == game.VenueId) model.IsVenueOwner = true;
            }
            else if (_gameService.IsCreatorOfGame(contact.ContactId, game)) 
            {
                model.IsCreatorOfGame = true;
                model.IsVenueOwner = false;
            }
            else
            {
                // Check if logged in user is already a part of this game
                var allPickUpGames = _gameService.GetPickUpGamesByContactId(contact.ContactId);
                if (allPickUpGames != null)
                {
                    var existingPickUpGame = allPickUpGames.FirstOrDefault(x => x.GameId == id);
                    if (existingPickUpGame == null) model.IsAlreadyJoined = false;
                    else model.IsAlreadyJoined = true;
                }
                else
                {
                    model.IsAlreadyJoined = false;
                }
            }

            model.EndDate = game.EndTime.ToString();
            model.GameId = game.GameId;
            model.Status = Enum.GetName(typeof(GameStatusEnum), game.GameStatusId);
            model.Sport = _gameService.GetSportNameById(game.SportId);
            model.StartDate = game.StartTime.ToString();
            model.Venue = _venueService.GetVenueNameById(game.VenueId);

            if (game.ContactId != null)
            {
                model.ContactId = game.ContactId;
                model.ContactName = _contactService.GetContactById(game.ContactId).Username;
            }

            //returning model to the view
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GameDetails(ViewGameViewModel model, string button)
        {
            //Find all the players that are currently signed up for the game
            List<PickUpGame> checkGames = _gameService.GetPickUpGameListByGameId(model.GameId);

            Game game = _gameService.GetGameById(model.GameId);

            //find the current logged-on user
            string email = User.Identity.GetUserName();
            Contact currContactUser = _contactService.GetContactByEmail(email);

            ViewGameViewModel returnModel = new ViewGameViewModel()
            {
                EndDate = game.EndTime.ToString(),
                GameId = game.GameId,
                Status = Enum.GetName(typeof(GameStatusEnum), game.GameStatusId),
                Sport = _gameService.GetSportNameById(game.SportId),
                StartDate = game.StartTime.ToString(),
                Venue = _venueService.GetVenueNameById(game.VenueId)
            };

            //make the email more readable
            var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailFormat.html"));
            //add game link to the email
            var directUrl = Url.Action("GameDetails", "Game", new { id = game.GameId }, protocol: Request.Url.Scheme);
            fileContents = fileContents.Replace("{URL}", directUrl);
            fileContents = fileContents.Replace("{URLNAME}", "CHECK");
            fileContents = fileContents.Replace("{VENUE}", returnModel.Venue);
            fileContents = fileContents.Replace("{SPORT}", _gameService.GetSportNameById(game.SportId));
            fileContents = fileContents.Replace("{STARTTIME}", returnModel.StartDate);
            fileContents = fileContents.Replace("{ENDTIME}", returnModel.EndDate);

            //If the Reject button was pressed
            if (button.Equals("Reject Game"))
            {               
                returnModel.Status = Enum.GetName(typeof(GameStatusEnum), 4);
                if (game.ContactId != null)
                {
                    returnModel.ContactId = game.ContactId;
                    returnModel.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }                
              
                //replace the html contents to the game details      
                fileContents = fileContents.Replace("{TITLE}", "Game Rejected");
                fileContents = fileContents.Replace("{INFO}", "Sorry the venue owner reject your game based on the confliction of the venue arrangement");                
                var subject = "Game Status";
                SendMessage(game, (int)returnModel.ContactId, fileContents,subject);

                //save it       
                _gameService.RejectGame(game.GameId);
            }
            //If the Reject button was pressed
            if (button.Equals("Accept Game"))
            {
                returnModel.Status = Enum.GetName(typeof(GameStatusEnum), 3);

                if (game.ContactId != null)
                {
                    returnModel.ContactId = game.ContactId;
                    returnModel.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                //replace the html contents to the game details      
                fileContents = fileContents.Replace("{TITLE}", "Game Accepted");
                fileContents = fileContents.Replace("{INFO}", "The venue owner accept your game!");
                var subject = "Game Status";
                SendMessage(game, (int)returnModel.ContactId, fileContents,subject);

                //save it       
                _gameService.AcceptGame(game.GameId);
            }           

            //If the Join Game button was pressed 
            if (button.Equals("Join Game"))
            {
                if (game.ContactId != null)
                {
                    returnModel.ContactId = game.ContactId;
                    returnModel.ContactName = _contactService.GetContactById(game.ContactId).Username;
                }

                //if the game is cancelled, users are prevent to join this game
                if (game.GameStatusId == 2)
                {
                    //error message
                    ViewData.ModelState.AddModelError("SignedUp", "Sorry, this game is canceled, you can not join this game");
                    return View(returnModel);
                }
                //check if the person is already signed up for the game 
                if (!_gameService.IsNotSignedUpForGame(currContactUser.ContactId, checkGames))
                {
                    //error message
                    ViewData.ModelState.AddModelError("SignedUp", "You are already signed up for this game");

                    return View(returnModel);
                }

                //add new person to the pickupgame table
                PickUpGame newPickUpGame = new PickUpGame()
                {
                    ContactId = currContactUser.ContactId,
                    GameId = model.GameId,
                };
                //save it       
                _gameService.AddPlayerToGame(newPickUpGame);

                //replace the html contents to the game details                     
                fileContents = fileContents.Replace("{TITLE}", "New Player!!!");
                fileContents = fileContents.Replace("{INFO}", currContactUser.Username + " has just joined the game." + "The current number of players on this game is: " + _gameService.GetPickUpGameListByGameId(game.GameId).Count());
                var subject = "Game Information Update";
                SendMessage(game, (int)returnModel.ContactId, fileContents, subject);                
            }

            //If the Leave Game button was pressed 
            if (button.Equals("Leave Game"))
            {              
                //check if the person is already signed up for the game 
                if (_gameService.IsNotSignedUpForGame(currContactUser.ContactId, checkGames))
                {
                    //error message
                    ViewData.ModelState.AddModelError("SignedUp", "You have not signed up for this game");

                    if (game.ContactId != null)
                    {
                        returnModel.ContactId = game.ContactId;
                        returnModel.ContactName = _contactService.GetContactById(game.ContactId).Username;
                    }

                    return View(returnModel);
                }

                Debug.Write(model);

                //Remove the Player from the Game 
                var usersGames = _gameService.GetPickUpGamesByContactId(currContactUser.ContactId);
                var pickUpGame = usersGames.FirstOrDefault(x => x.GameId == model.GameId);
                _gameService.RemovePlayerFromGame(pickUpGame);

                //count the number of current users in the game to send notification to the creator
                int playerCount = 0;

                if (_gameService.GetPickUpGameListByGameId(game.GameId) == null)
                {
                    playerCount = 0;
                }
                else
                {
                    playerCount = _gameService.GetPickUpGameListByGameId(game.GameId).Count();
                }

                //replace the html contents to the game details                     
                fileContents = fileContents.Replace("{TITLE}", "Somebody Leaves");
                fileContents = fileContents.Replace("{INFO}", currContactUser.Username + " has just left the game. " + "The current number of players on this game is: " + playerCount);
                var subject = "Game Information Update";
                SendMessage(game, (int)game.ContactId, fileContents, subject);                                   
            }

            return RedirectToAction("GameDetails", new { id = model.GameId });
        }

        public void SendMessage(Game game, int playerId, string body, string subject)
        {
            //Initializing Message Details 
            string sendingToEmail = "";
            string messageContent = "";

            //Either sending the message to the Creator of the game or the Players in the game
            if (game.ContactId == playerId)
            {
                sendingToEmail = _contactService.GetContactById((int)game.ContactId).Email;
                messageContent = body;

            }
            else
            {
                if (_contactService.GetContactById(playerId) != null)
                {
                    //emailing to the players on the game list
                    sendingToEmail = _contactService.GetContactById(playerId).Email;
                }
                else
                {
                    //email to the venue owner
                    sendingToEmail = _venueOwnerService.GetVenueOwnerById(playerId).Email;
                }
                messageContent = body;
            }

            MailMessage mailMessage = new MailMessage(_gMailer.GetEmailAddress(), sendingToEmail)
            {
                Body = messageContent,
                Subject = subject

            };

            //Send the Message
            _gMailer.Send(mailMessage);
        }
        
        /**
         * Partial view that to display business hours on CreateGame page
         */
        [HttpGet]
        public PartialViewResult BusinessHoursByVenueId(int id)
        {
            // Map business hours
            List<BusinessHours> businessHours = _venueService.GetVenueBusinessHours(id);
            List<BusinessHoursViewModel> model = new List<BusinessHoursViewModel>();

            foreach (var businessHour in businessHours)
            {
                var closeDateTime = new DateTime() + businessHour.CloseTime;
                var openDateTime = new DateTime() + businessHour.OpenTime;

                model.Add(new BusinessHoursViewModel
                {
                    DayOfWeek = Enum.GetName(typeof(DayOfWeek), businessHour.DayOfWeek),
                    CloseTime = closeDateTime.ToShortTimeString(),
                    OpenTime = openDateTime.ToShortTimeString()
                });
            }

            Venue venue = _venueService.GetVenueById(id);

            if (!_venueService.VenueHasOwner(venue))
            {
                ViewData.ModelState.AddModelError("NoOwner", "This venue has not been claimed by an owner, please be sure to contact them directly after creating your game to avoid scheduling conflicts");
            }

            // Partial view displaying bids for specific item
            return PartialView("_BusinessHours", model);
        }

        public PartialViewResult GetGamesResult(int venueId)
        {
            //list of games found using venue ID
            var gameList = _gameService.GetCurrentGamesByVenueId(venueId);

            if (gameList == null || gameList.Count == 0)
            {
                ViewData.ModelState.AddModelError("GameSearch", "There are no games that matches your search");
                return PartialView("_GameSearch");
            }

            //List using ViewModel to format how I like 
            List<GameListViewModel> model = new List<GameListViewModel>();

            //Find right data for each variable 
            foreach (var game in gameList)
            {
                var gameToAdd = new GameListViewModel
                {
                    GameId = game.GameId,
                    Sport = _gameService.GetSportNameById(game.SportId),
                    Venue = _venueService.GetVenueNameById(game.VenueId),
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


            //returning it back to my Ajax js method
            return PartialView("_GameSearch", model);

        }

        public PartialViewResult SearchBySport(int sportId)
        {
            List<Game> gameList = _gameService.GetCurrentGamesBySportId(sportId);

            if (gameList.Count == 0)
            {
                ViewData.ModelState.AddModelError("GameSearch", "There are no games that matches your search");
            }

            //List using ViewModel to format how I like 
            List<GameListViewModel> model = new List<GameListViewModel>();


            //Find right data for each variable 
            foreach (var game in gameList)
            {
                var gameToAdd = new GameListViewModel
                {
                    GameId = game.GameId,
                    Sport = _gameService.GetSportNameById(game.SportId),
                    Venue = _venueService.GetVenueNameById(game.VenueId),
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
            return PartialView("_GameSearch", model);
        }

        public PartialViewResult TimeFilter(string dateRange)
        {
            var dates = dateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.InvariantCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            List<GameListViewModel> model = new List<GameListViewModel>();
            foreach (var game in _gameService.GetAllCurrentOpenGames())
            {
                if (_gameService.IsSelectedTimeValid(startDateTime, endDateTime))
                {
                    if (game.StartTime >= startDateTime && game.EndTime <= endDateTime)
                    {
                        var gameToAdd = new GameListViewModel
                        {
                            GameId = game.GameId,
                            Sport = _gameService.GetSportNameById(game.SportId),
                            Venue = _venueService.GetVenueNameById(game.VenueId),
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
                }
                else
                {
                    var gameToAdd = new GameListViewModel
                    {
                        GameId = game.GameId,
                        Sport = _gameService.GetSportNameById(game.SportId),
                        Venue = _venueService.GetVenueNameById(game.VenueId),
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
            }                         
            if (model.Count == 0)
            {
                ViewData.ModelState.AddModelError("GameSearch", "There are no games that matches your search");
            }
            return PartialView("_GameSearch", model);
        }

        public PartialViewResult PlayerList(int gameId)
        {
            List<PickUpGame> playerList = _gameService.GetPickUpGameListByGameId(gameId);

            List<PickUpGameViewModel> model = new List<PickUpGameViewModel>();

            if (playerList == null) return PartialView("_PlayerList", null);

            foreach (var player in playerList)
            {
                var contact = _contactService.GetContactById(player.ContactId);

                model.Add(new PickUpGameViewModel
                {
                    PickUpGameId = player.PickUpGameId,
                    GameId = gameId,
                    Username = contact.Username
                });

            }

            return PartialView("_PlayerList", model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditGame(int id)
        {
            //Get current users email
            string email = User.Identity.GetUserName();

            //Find user by their email
            Contact currContact = _contactService.GetContactByEmail(email);

            //Find Game
            Game game = _gameService.GetGameById(id);

            //Populate dropdown using the current values of the existing game
            PopulateEditDropDownMethod(game);

            //check if the user is the creator of the game
            if (!_gameService.IsCreatorOfGame(currContact.ContactId, game))
            {
                return RedirectToAction("GameDetails", new {id = id});
            }

            Debug.Write(game);

            //Convert the strings so it matches
            string dateRange = game.StartTime.ToString("MM/dd/yyyy hh:mm tt") + " - " + game.EndTime.ToString("MM/dd/yyyy hh:mm tt");

            ////Create own View Model
            EditGameViewModel model = new EditGameViewModel()
            { 
                GameId = id,
                ContactId = (int) game.ContactId,
                VenueId = game.VenueId,
                SportId = game.SportId,
                Sport = _gameService.GetSportNameById(game.SportId),
                Venue = _venueService.GetVenueNameById(game.VenueId),
                Status = Enum.GetName(typeof(GameStatusEnum), game.GameStatusId),
                DateRange = dateRange,

            };

            //pass it back
            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult EditGame(EditGameViewModel model)
        {
            //checking model state
            if (!ModelState.IsValid) return View(model);

            //Parse the date ranges
            var dates = model.DateRange.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var startDateTime = DateTime.Parse(dates[0], CultureInfo.InvariantCulture);
            var endDateTime = DateTime.Parse(dates[1], CultureInfo.CurrentCulture);

            //find the existing game
            Game existingGame = _gameService.GetGameById(model.GameId);

            //populating dropdown with the existing game
            PopulateEditDropDownMethod(existingGame);

            //check if the game will happen in one hour
            if (!_gameService.IsThisGameCanCancel(startDateTime))
            {
                ViewData.ModelState.AddModelError("GameStart", "Sorry, you can only edit the game at least 1 hour long before it starts.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //check if this game rejected
            if (_gameService.GetGameById(model.GameId).GameStatusId==4)
            {
                ViewData.ModelState.AddModelError("GameRejected", "Sorry, this game is rejected by the venue owner so you cannot edit this game.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //check if the dates match - Can't have games that start and end on different dates 
            if (!_gameService.IsSelectedTimeValid(startDateTime, endDateTime))
            {
                ViewData.ModelState.AddModelError("DateRange", "Start date and end date must be same date.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //check for existing games
            Game gameCheck = _gameService.CheckForExistingGameExceptItself(model.VenueId, model.SportId, startDateTime, model.GameId);
            if (gameCheck != null)
            {
                ViewData.ModelState.AddModelError("GameExists", "A game already exists with this venue, sport, and time.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            // Get venue by ID and business hours for that venue
            Venue venue = _venueService.GetVenueById(model.VenueId);
            List<BusinessHours> venueHours = _venueService.GetVenueBusinessHours(model.VenueId);

            // Return error to View if the venue is not available
            bool isVenueAvailable = _venueService.IsVenueAvailable(venueHours, startDateTime, endDateTime);
            if (!isVenueAvailable)
            {
                ViewData.ModelState.AddModelError("DateRange", $"Unfortunately, {venue.Name} is not available during the hours you chose.");
                PopulateEditDropDownMethod(existingGame);
                return View(model);
            }

            //if all the checks passed, then pass the values into the existing game
            var gameStatusEnum = (GameStatusEnum) Enum.Parse(typeof(GameStatusEnum), model.Status);
            existingGame.VenueId = model.VenueId;
            existingGame.SportId = model.SportId;
            existingGame.GameStatusId = (int) gameStatusEnum;
            existingGame.GameId = model.GameId;
            existingGame.ContactId = model.ContactId;
            existingGame.StartTime = startDateTime;
            existingGame.EndTime = endDateTime;

            _gameService.EditGame(existingGame);

            //send email to users once the game is cancelled
            if (int.Parse(model.Status) == (int) GameStatusEnum.Cancelled)
            {
                var venueName = _venueService.GetVenueNameById(existingGame.VenueId);
                var sportName = _gameService.GetSportNameById(existingGame.SportId);

                var fileContents = System.IO.File.ReadAllText(Server.MapPath("~/Content/EmailFormat.html"));
                //add game link to the email
                var directUrl = Url.Action("GameDetails", "Game", new { id = existingGame.GameId }, protocol: Request.Url.Scheme);
                fileContents = fileContents.Replace("{URL}", directUrl);
                fileContents = fileContents.Replace("{URLNAME}", "CHECK");
                fileContents = fileContents.Replace("{VENUE}", venueName);
                fileContents = fileContents.Replace("{SPORT}", sportName);
                fileContents = fileContents.Replace("{STARTTIME}", startDateTime.ToString());
                fileContents = fileContents.Replace("{ENDTIME}", endDateTime.ToString());
                fileContents = fileContents.Replace("{TITLE}", "Game Cancelled");
                fileContents = fileContents.Replace("{INFO}", "Sorry, this game is canceled by the creator.");
                var subject = "Game Cancelled";


                List<PickUpGame> pickUpGameList = _gameService.GetPickUpGameListByGameId(existingGame.GameId);

                // If empty, don't need to email people
                if (pickUpGameList != null)
                {
                    foreach (var player in pickUpGameList)
                    {
                        SendMessage(existingGame, player.ContactId, fileContents, subject);
                    }
                }                        
            }

            //redirect them back to the changed game detail
            return RedirectToAction("GameDetails", new {id = model.GameId});
        }
        
        /**
         * Helper methods
        */
        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _venueService.GetAllVenues().ToDictionary(v => v.VenueId, v => v.Name);
            ViewBag.Sports = _gameService.GetAllSports().ToList().ToDictionary(s => s.SportID, s => s.SportName);
        }
        
        /**
         * Helper method that takes in the game object and creates a dropdown with the current game values
         */
        public void PopulateEditDropDownMethod(Game game)
        {
            ViewBag.Venue = new SelectList(_venueService.GetAllVenues(), "VenueId", "Name", game.VenueId);
            ViewBag.Sport = new SelectList(_gameService.GetAllSports(), "SportId", "SportName", game.SportId);

            // Take out Accepted and Rejected statuses since reserved for venue owner
            var statuses = _gameService.GetAllGameStatuses();
            statuses.RemoveAll(status => status.Status == "Accepted");
            statuses.RemoveAll(status => status.Status == "Rejected");
            
            ViewBag.Status = new SelectList(statuses, "GameStatusId", "Status", game.GameStatusId);
        }
    }
}