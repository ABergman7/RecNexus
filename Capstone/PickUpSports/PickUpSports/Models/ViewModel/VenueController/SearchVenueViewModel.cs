using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel.VenueController
{
    public class SearchVenueViewModel
    {
        public string Search { get; set; }

        public string Filter { get; set; }

        public string CurrentLatitude { get; set; } = "44.942898";

        public string CurrentLongitude { get; set; } = "-123.035095";

        public string Day { get; set; }

        public string Time { get; set; }

        public List<VenueViewModel> Venues { get; set; }
    }
}