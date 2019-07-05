using System;

namespace PickUpSports.Models.DatabaseModels
{
    public class Game
    {
        public int GameId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        
        public int VenueId { get; set; }

        public int SportId { get; set; }

        public int? ContactId { get; set; }

        public int GameStatusId { get; set; }
    }
}