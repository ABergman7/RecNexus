using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel.VenueController
{
    public class PartialReviewViewModel
    {
        public int VenueId { get; set; }

        public decimal? AverageReviewRating { get; set; }

        public List<ReviewViewModel> Reviews { get; set; }
    }
}