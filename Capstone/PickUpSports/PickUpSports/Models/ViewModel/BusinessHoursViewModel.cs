using System;
using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.ViewModel
{
    public class BusinessHoursViewModel
    {
        public string DayOfWeek { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }
    }
}