using System;

namespace PickUpSports.Models.DatabaseModels
{
    public class Review
    {
        public int ReviewId { get; set; }

        public DateTime Timestamp { get; set; }

        public int Rating { get; set; }

        public string Comments { get; set; }

        public bool IsGoogleReview { get; set; }

        public string GoogleAuthor { get; set; }

        public int? ContactId { get; set; }

        public int VenueId { get; set; }
    }

}