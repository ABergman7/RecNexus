using System;

namespace PickUpSports.Models.ViewModel.VenueOwnerController
{
    public class EventViewModel
    {
        public int GameId { get; set; }

        public string Subject { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Color { get; set; }

        public string GameStatus { get; set; }
    }
}