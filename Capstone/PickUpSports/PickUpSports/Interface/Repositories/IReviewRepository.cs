using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IReviewRepository
    {
        Review AddReview(Review review);

        Review GetReviewById(int id);

        void EditReview(Review review);

        List<Review> GetReviewsByContactId(int contactId);

        List<Review> GetAllReviews();

        void DeleteReview(Review review);
    }
}