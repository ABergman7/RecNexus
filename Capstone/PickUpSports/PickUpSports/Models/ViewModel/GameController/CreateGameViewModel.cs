using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class CreateGameViewModel
    {
        [Required]
        [DisplayName("Pick a start and finish time (must start and end on same date):")]
        public string DateRange { get; set; }

        [Required]
        [DisplayName("Venue")]
        public int VenueId { get; set; }

        [Required]
        [DisplayName("Sport")]
        public int SportId { get; set; }
    }
}