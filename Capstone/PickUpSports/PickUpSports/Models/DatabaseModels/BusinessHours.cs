using System;

namespace PickUpSports.Models.DatabaseModels
{
    public class BusinessHours
    {
        public int BusinessHoursId { get; set; }

        public int DayOfWeek { get; set; }

        public TimeSpan OpenTime { get; set; }

        public TimeSpan CloseTime { get; set; }

        public int VenueId { get; set; }
    }
}