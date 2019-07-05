using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly PickUpContext _context;

        public ReviewRepository(PickUpContext context)
        {
            _context = context;
        }

        public Review AddReview(Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return review;
        }

        public Review GetReviewById(int id)
        {
            return _context.Reviews.Find(id);
        }

        public void EditReview(Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public List<Review> GetReviewsByContactId(int contactId)
        {
            List<Review> reviews = _context.Reviews.Where(s => s.ContactId == contactId).ToList();
            return reviews;
        }

        public List<Review> GetAllReviews()
        {
            return _context.Reviews.ToList();
        }

        public void DeleteReview(Review review)
        {
            Review existing = _context.Reviews.Find(review.ReviewId);
            if (existing == null) throw new ArgumentNullException($"Could not find review by ID {review.ReviewId}");

            _context.Reviews.Remove(existing);
            _context.SaveChanges();
        }
    }
}