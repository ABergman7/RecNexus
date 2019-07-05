using System.Collections.Generic;

namespace PickUpSports.Models.ViewModel.ContactController
{
    public class CreateSportPreferenceViewModel
    {
        public int ContactId { get; set; }

        public string ContactUsername { get; set; }

        public List<SelectSportPreferenceViewModel> SportPreferenceCheckboxes { get; set; }
    }
}