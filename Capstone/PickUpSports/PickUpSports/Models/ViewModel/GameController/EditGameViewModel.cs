using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.ViewModel.GameController
{
    public class EditGameViewModel
    {
        public int GameId { get; set; }

        public int ContactId { get; set; }

        public int VenueId { get; set; }

        public int SportId { get; set; }

        public int GameStatusId { get; set; }

        [Required]
        [DisplayName("Venue:")]
        public string Venue { get; set; }

        [Required]
        [DisplayName("Sport:")]
        public string Sport { get; set; }

        [Required]
        [DisplayName("Status:")]
        public string Status { get; set; }

        [Required]
        [DisplayName("Pick a start and finish time (must start and end on same date):")]
        public string DateRange { get; set; }

        public string StartTime { get; set; }
    }
}