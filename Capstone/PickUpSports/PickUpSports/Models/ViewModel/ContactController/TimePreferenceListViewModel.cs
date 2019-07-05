using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel.ContactController
{
    public class TimePreferenceListViewModel
    {
        public int ContactId { get; set; }

        public List<TimePreferenceViewModel> TimePreferences { get; set; }

        public bool IsPublicProfileView { get; set; }

    }
}