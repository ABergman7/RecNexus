using System;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class GameListViewModel
    {
        public int? ContactId { get; set; }

        public string ContactName { get; set; }

        public int GameId { get; set; }

        public int? VenueId { get; set; }

        public string Venue { get; set; }

        public string Sport { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

          
        public string GameStatus { get; set; }
    }
}