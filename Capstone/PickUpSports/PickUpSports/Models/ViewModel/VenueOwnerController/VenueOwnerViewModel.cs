using System;

namespace PickUpSports.Models.ViewModel.VenueOwnerController
{
    public class VenueOwnerViewModel
    {
        public int VenueOwnerId { get; set; }

        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string CompanyName { get; set; }

        public DateTime SignUpDate { get; set; }
    }
}