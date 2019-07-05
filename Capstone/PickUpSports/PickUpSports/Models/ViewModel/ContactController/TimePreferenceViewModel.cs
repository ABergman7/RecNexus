using System;

namespace PickUpSports.Models.ViewModel.ContactController
{
    public class TimePreferenceViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }

        public TimeSpan? BeginTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}