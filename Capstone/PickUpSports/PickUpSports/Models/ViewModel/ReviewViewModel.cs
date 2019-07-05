using System;

namespace PickUpSports.Models.ViewModel
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }

        public DateTime Timestamp { get; set; }

        public int Rating { get; set; }

        public string Comments { get; set; }

        public string Author { get; set; }

        public bool ReviewBelongsToUser { get; set; } = false;
    }
}