using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PickUpSports.Interface;
using PickUpSports.Models;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;
using PickUpSports.Models.ViewModel.VenueOwnerController;

namespace PickUpSports.Controllers
{
    public class VenueOwnerController : Controller
    {
        private readonly IGMailService _gMailer;
        private readonly IVenueOwnerService _venueOwnerService;
        private readonly IVenueService _venueService;
        private ApplicationUserManager _userManager;
        private readonly IGameService _gameService;
        private readonly IContactService _contactService;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public VenueOwnerController(IGMailService gMailer, 
            IVenueOwnerService venueOwnerService, 
            IVenueService venueService, IGameService gameService, IContactService contactService)
        {
            _gMailer = gMailer;
            _venueOwnerService = venueOwnerService;
            _venueService = venueService;
            _gameService = gameService;
            _contactService = contactService;
        }

        public ActionResult ClaimVenue(int venueId)
        {
            var model = new ClaimVenueFormViewModel();
            model.VenueId = venueId;
            model.VenueName = _venueService.GetVenueNameById(venueId);

            ViewBag.SubmitEmailSent = false;
            return View(model);
        }

        [HttpPost]
        public ActionResult ClaimVenue(ClaimVenueFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SubmitEmailSent = false;
                return View(model);
            }

            // Send email to Scrum Lords for approval
            var email = new MailMessage();
            email.To.Add(_gMailer.GetEmailAddress());
            email.From = new MailAddress(model.Email);
            email.Subject = $"Received request to claim venue {model.VenueName}.";
            email.Body = $"Received below request to claim venue {model.VenueName}:<br />"
                         + $"Name: {model.FirstName} {model.LastName}<br />"
                         + $"Email: {model.Email}<br />"
                         + $"Phone: {model.PhoneNumber}<br />"
                         + $"Company name (if applicable): {model.CompanyName}<br />"
                         + "Proof of ownership document is attached.";
            email.IsBodyHtml = true;

            var attachment = new Attachment(model.Document.InputStream, model.Document.FileName);
            email.Attachments.Add(attachment);

            // Send email and return to view if successful
            if (_gMailer.Send(email))
            {
                ViewBag.SubmitEmailSent = true;
                return View(model);
            }

            // Email could not be sent, display error 
            ViewBag.SubmitEmailSent = false;
            ViewData.ModelState.AddModelError("EmailError", "Unfortunately, your email could not be sent.");
            return View(model);
        }

        [Authorize(Roles="Admin")]
        public ActionResult GetAllVenuOwners()
        {
            var allOwners = _venueOwnerService.GetAllVenueOwners();
            var model = new List<VenueOwnerViewModel>();

            foreach (var venueOwner in allOwners)
            {
                model.Add(new VenueOwnerViewModel
                {
                    FirstName = venueOwner.FirstName,
                    LastName = venueOwner.LastName,
                    CompanyName = venueOwner.CompanyName,
                    Email = venueOwner.Email,
                    PhoneNumber = venueOwner.Phone,
                    SignUpDate = venueOwner.SignUpDate,
                    VenueName = _venueService.GetVenueNameById(venueOwner.VenueId),
                    VenueOwnerId = venueOwner.VenueOwnerId,
                    VenueId = venueOwner.VenueId
                });
            }

            return PartialView("_ModifyVenueOwners", model);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            //Return drop down of the venues
            PopulateDropdownValues();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CreateVenueOwnerViewModel model)
        {
            PopulateDropdownValues();

            //if model state is invalid return
            if (!ModelState.IsValid)
            {
                PopulateDropdownValues();
                return View(model);
            }

            //Find venue with the view model id
            Venue venue = _venueService.GetVenueById(model.VenueId);

            //If the venue already has an owner, error 
            if (_venueOwnerService.VenueHasOwner(venue))
            {
                ViewData.ModelState.AddModelError("OwnerExists", "There is already an owner for this Venue");
                PopulateDropdownValues();
                return View(model);
            }

            // Adding owner to AspNetUsers table
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true};

            var temporaryPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["VenueOwnerTemporaryPassword"];
            var result = UserManager.Create(user, temporaryPassword);
            if (result.Succeeded)
            {
                //Creating new owner with the values from the view
                VenueOwner owner = new VenueOwner
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.PhoneNumber,
                    CompanyName = model.CompanyName,
                    SignUpDate = DateTime.Now,
                    VenueId = venue.VenueId
                };

                //Add the owner to the table, and save changes
                _venueOwnerService.AddVenueOwner(owner);

                // Only admin can create venue owner so return to admin dashboard
                return RedirectToAction("Index", "Admin");
            }
            
            ViewData.ModelState.AddModelError("AddError", "Could not add venue owner.");
            PopulateDropdownValues();
            return View(model);
        }

        [Authorize]
        public ActionResult Detail(int? id)
        {
            //Bool variable for the View: checking if user is venue owner or not
           ViewBag.IsOwner = false;

            //Get the current logged in user - will be implemented in the future when the log in function/ page is complete
            string ownerEmail = User.Identity.GetUserName();

            //Check if current logged in user is the owner
            if (_venueOwnerService.IsVenueOwner(ownerEmail))
            {
                ViewBag.IsOwner = true;

                VenueOwner owner = _venueOwnerService.GetVenueOwnerByEmail(ownerEmail);

                //Create the view model using values of the owner if not null
                CreateVenueOwnerViewModel model = new CreateVenueOwnerViewModel()
                {
                    VenueOwnerId = owner.VenueOwnerId,
                    FirstName = owner.FirstName,
                    LastName = owner.LastName,
                    Email = owner.Email,
                    PhoneNumber = owner.Phone,
                    CompanyName = owner.CompanyName,
                    SignUpDate = owner.SignUpDate,
                    VenueName = _venueService.GetVenueNameById(owner.VenueId),
                    VenueId = owner.VenueId
                };

                //Return it back to the view
                return View(model);
            }
            else
            {
                ViewBag.IsOwner = false;
                VenueOwner owner = _venueOwnerService.GetVenueOwnerById((int) id);

                CreateVenueOwnerViewModel model = new CreateVenueOwnerViewModel()
                {
                    VenueOwnerId = owner.VenueOwnerId,
                    FirstName = owner.FirstName,
                    LastName = owner.LastName,
                    Email = owner.Email,
                    PhoneNumber = owner.Phone,
                    CompanyName = owner.CompanyName,
                    SignUpDate = owner.SignUpDate,
                    VenueName = _venueService.GetVenueNameById(owner.VenueId),
                    VenueId = owner.VenueId
                };
                return View(model);

            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            //Bad request if value is 0
            if (id == 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Find venue owner using the id
            VenueOwner owner = _venueOwnerService.GetVenueOwnerById(id);

            //Initialize view model values using the owner values
            CreateVenueOwnerViewModel model = new CreateVenueOwnerViewModel()
            { 
                VenueOwnerId = owner.VenueOwnerId,
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Email = owner.Email,
                PhoneNumber = owner.Phone,
                CompanyName = owner.CompanyName,
                SignUpDate = owner.SignUpDate,
                VenueName = _venueService.GetVenueNameById(owner.VenueId)
            };

            //Return view back
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(CreateVenueOwnerViewModel model)
        {
            //Find the existing owner withe the Id
            VenueOwner existingOwner = _venueOwnerService.GetVenueOwnerById(model.VenueOwnerId);

            //Add the changed values to the existing owner
            existingOwner.VenueOwnerId = model.VenueOwnerId;
            existingOwner.FirstName = model.FirstName;
            existingOwner.LastName = model.LastName;
            existingOwner.Email = model.Email;
            existingOwner.Phone = model.PhoneNumber;
            existingOwner.CompanyName = model.CompanyName;

            //save the changes
            _venueOwnerService.EditVenueOwner(existingOwner);

            // An administrator is editing, return to Admin dashboard
            if (User.IsInRole("Admin")) return RedirectToAction("Index", "Admin");

            //Redirect to the details page 
            return RedirectToAction("Detail", "VenueOwner", new {id = existingOwner.VenueOwnerId});
        }

        [HttpPost]
        public ActionResult MessageOwner(CreateVenueOwnerViewModel model)
        {
            //Get the current logged in users email
            string currUserEmail = User.Identity.GetUserName();

            //Find the Venue owner using the Id
            VenueOwner owner = _venueOwnerService.GetVenueOwnerById(model.VenueOwnerId);

            string subject = "Message from User Concerning Your Venue: " + model.MessageSubject;
            string body = "<b>" + currUserEmail + "</b> wrote: <i>" + model.MessageBody + "</i>";

            //Create a new message with the inputs
            MailMessage mail = new MailMessage(_gMailer.GetEmailAddress(), owner.Email)
            {
                Subject = subject,
                Body = body
            };

            mail.IsBodyHtml = true;

            //Send Message
            _gMailer.Send(mail);

            return RedirectToAction("Detail", "VenueOwner", new { id = model.VenueOwnerId });
        }

        public JsonResult GetEvents(int id)
         {
            var gamesAtVenue = _gameService.GetCurrentGamesByVenueId(id);
            var events = new List<EventViewModel>();

            foreach (var game in gamesAtVenue)
            {
                var sportName = _gameService.GetSportNameById(game.SportId);
                var username = _contactService.GetUsernameByContactId((int) game.ContactId);

                var gameEvent = new EventViewModel();
                gameEvent.Subject = $"{sportName} game created by {username}";
                gameEvent.Start = game.StartTime;
                gameEvent.End = game.EndTime;
                gameEvent.GameId = game.GameId;
                gameEvent.GameStatus = ((GameStatusEnum) game.GameStatusId).ToString();

                // Color coding
                if (game.GameStatusId == (int)GameStatusEnum.Cancelled) gameEvent.Color = "Grey";
                if (game.GameStatusId == (int)GameStatusEnum.Rejected) gameEvent.Color = "Red";

                events.Add(gameEvent);
            }

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /*
         * A venue owner can reject any game at their venue 
         */
        public ActionResult RejectGame(int id)
        {
            // Email creator game that game is being cancelled
            var game = _gameService.GetGameById(id);
            var owner = _venueOwnerService.GetVenueOwnerByVenueId(game.VenueId);
            if (game.ContactId != null)
            {
                var creator = _contactService.GetContactById(game.ContactId);
                var venueOwnerLink = "<a href='"
                           + Request.Url.Scheme + "://"
                           + Request.Url.Authority
                           + @Url.Action("Detail", "VenueOwner", new {id = owner.VenueOwnerId})
                           + "'>Visit this page for owner's contact information</a>";

                var venueName = _venueService.GetVenueNameById(game.VenueId);
                var sport = _gameService.GetSportNameById(game.SportId);

                var email = new MailMessage();
                email.To.Add(creator.Email);
                email.From = new MailAddress(_gMailer.GetEmailAddress());
                email.Subject = $"We apologize. The owner of {venueName} has cancelled your game.";
                email.Body = $"<center>Unfortunately, the owner of {venueName} has cancelled your "
                             + $"{sport.ToLower()} game that you scheduled on {game.StartTime.ToShortDateString()}."
                             + $"<p>{venueOwnerLink}</p></center>";
                email.IsBodyHtml = true;

                _gMailer.Send(email);
            }

            _gameService.RejectGame(id);
            
            return RedirectToAction("Detail");
        }

        /*
         * If a game was once rejected, only the venue owner can re-accept it
         */
        public ActionResult AcceptGame(int id)
        {
            // Email creator game that game is being reinstated by owner
            var game = _gameService.GetGameById(id);
            var owner = _venueOwnerService.GetVenueOwnerByVenueId(game.VenueId);
            if (game.ContactId != null)
            {
                var creator = _contactService.GetContactById(game.ContactId);
                var venueOwnerLink = "<a href='"
                                     + Request.Url.Scheme + "://"
                                     + Request.Url.Authority
                                     + @Url.Action("Detail", "VenueOwner", new { id = owner.VenueOwnerId })
                                     + "'>Visit this page for owner's contact information</a>";

                var venueName = _venueService.GetVenueNameById(game.VenueId);
                var sport = _gameService.GetSportNameById(game.SportId);

                var email = new MailMessage();
                email.To.Add(creator.Email);
                email.From = new MailAddress(_gMailer.GetEmailAddress());
                email.Subject = $"Good news! The owner of {venueName} has reinstated your game.";
                email.Body = $"<center>The owner of {venueName} has reinstated your previously rejected "
                             + $"{sport.ToLower()} game that you scheduled on {game.StartTime.ToShortDateString()}."
                             + $"<p>{venueOwnerLink}</p></center>";
                email.IsBodyHtml = true;

                _gMailer.Send(email);
            }

            _gameService.AcceptGame(id);

            return RedirectToAction("Detail");
        }

        /*
         * Helper method that creates the dropdown of the venues
         */
        public void PopulateDropdownValues()
        {
            ViewBag.Venues = _venueService.GetAllVenues().ToDictionary(v => v.VenueId, v => v.Name);
        }
    }
}
