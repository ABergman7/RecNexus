using System;

namespace PickUpSports.Models.DatabaseModels
{
    public class TimePreference
    {

        public int TimePrefID { get; set; }

        public int DayOfWeek { get; set; }

        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public int ContactID { get; set; }

    }

}