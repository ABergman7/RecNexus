using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PickUpSports.Interface;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel.ReviewController;

namespace PickUpSports.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IVenueService _venueService;
        private readonly IContactService _contactService;

        public ReviewController(IVenueService venueService, IContactService contactService)
        {
            _venueService = venueService;
            _contactService = contactService;
        }

        [Authorize]
        public ActionResult Create(int id)
        { 
            // Get name of venue to display on View and return model
            Venue venue = _venueService.GetVenueById(id);

            var model = new CreateReviewViewModel
            {
                VenueId = venue.VenueId,
                VenueName = venue.Name
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateReviewViewModel model)
        {
            // Get contact to tie to review
            string email = User.Identity.GetUserName();
            Contact contact = _contactService.GetContactByEmail(email);

            // Create and add Review to database
            Review review = new Review
            {
                IsGoogleReview = false,
                Timestamp = DateTime.Now,
                Comments = model.Comments,
                Rating = model.Rating,
                VenueId = model.VenueId,
                ContactId = contact.ContactId
            };
            
            _venueService.CreateVenueReview(review);

            // Redirect user to Venue details page which shows all reviews
            return RedirectToAction("Details", "Venue", new {id = model.VenueId});
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var review = _venueService.GetReviewById(id);
            
            var model = new EditReviewViewModel();
            model.VenueId = review.VenueId;
            model.Comments = review.Comments;
            model.Rating = review.Rating;
            model.ReviewId = review.ReviewId;
            model.VenueName = _venueService.GetVenueNameById(review.VenueId);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditReviewViewModel model)
        {
            var existingReview = _venueService.GetReviewById(model.ReviewId);

            // Get logged in user
            var email = User.Identity.Name;

            // Get review author
            var contact = _contactService.GetContactById(existingReview.ContactId);

            // Return error if users are not same
            if (!email.Equals(contact.Email))
            {
                ViewData.ModelState.AddModelError("Not Author", "You are not authorized to edit this review.");
                return View(model);
            }

            existingReview.Comments = model.Comments;
            existingReview.Rating = model.Rating;
            
            _venueService.EditVenueReview(existingReview);

            return RedirectToAction("Details", "Venue", new { id = model.VenueId });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var review = _venueService.GetReviewById(id);
            _venueService.DeleteVenueReview(review);

            return RedirectToAction("Details", "Venue", new { id = review.VenueId });
        }
    }
}