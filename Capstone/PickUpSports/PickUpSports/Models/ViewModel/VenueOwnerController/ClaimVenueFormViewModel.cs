using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PickUpSports.Models.ViewModel.VenueOwnerController
{
    public class ClaimVenueFormViewModel
    {
        [Required]
        [DisplayName("First name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Phone number")]
        public string PhoneNumber { get; set; }

        public string CompanyName { get; set; }

        public int VenueId { get; set; }

        public string VenueName { get; set; }

        [Required]
        public HttpPostedFileBase Document { get; set; }
    }
}