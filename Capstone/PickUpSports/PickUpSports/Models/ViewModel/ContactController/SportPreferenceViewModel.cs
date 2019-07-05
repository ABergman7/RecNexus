using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel.ContactController
{
    public class SportPreferenceViewModel
    {
        public int ContactId { get; set; }
        public List<string> SportName { get; set; }
        public bool IsPublicProfileView { get; set; }
    }
}