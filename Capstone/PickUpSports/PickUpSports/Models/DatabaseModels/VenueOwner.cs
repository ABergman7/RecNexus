using System;

namespace PickUpSports.Models.DatabaseModels
{
    public class VenueOwner
    {
        public int VenueOwnerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string CompanyName { get; set; }

        public DateTime SignUpDate { get; set; }

        public int VenueId { get; set; }
    }
}