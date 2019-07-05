namespace PickUpSports.Models.ViewModel.ReviewController
{
    public class CreateReviewViewModel
    {
        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public string Comments { get; set; }

        public int Rating { get; set; }
    }
}