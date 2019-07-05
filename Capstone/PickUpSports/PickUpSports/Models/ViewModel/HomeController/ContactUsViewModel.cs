using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.ViewModel.HomeController
{
    public class ContactUsViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }
    }
}