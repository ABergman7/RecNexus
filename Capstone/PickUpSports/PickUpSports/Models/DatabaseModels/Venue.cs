using System;
using System.Collections;
using System.Collections.Generic;

namespace PickUpSports.Models.DatabaseModels
{
    public class Venue
    {
        public int VenueId { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string GooglePlaceId { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }

}