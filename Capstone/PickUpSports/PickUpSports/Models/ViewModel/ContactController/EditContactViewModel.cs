using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.ViewModel.ContactController
{
    public class EditContactViewModel
    {
        public int ContactId { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string Username { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("Address 1")]
        public string Address1 { get; set; }

        [DisplayName("Address 2 (Optional)")]
        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        public string ZipCode { get; set; }

        public bool HasPublicProfile { get; set; }
    }
}