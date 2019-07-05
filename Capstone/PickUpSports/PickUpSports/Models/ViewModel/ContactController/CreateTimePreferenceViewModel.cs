using System;
using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.ViewModel.ContactController
{
    public class CreateTimePreferenceViewModel
    {
        public int TimePrefID { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage =
            "Time must be between 00:00 to 23:59")]
        public TimeSpan? BeginTime { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh\\:mm}")]
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage =
            "Time must be between 00:00 to 23:59")]
        public TimeSpan? EndTime { get; set; }

        public int ContactID { get; set; }

    }
}