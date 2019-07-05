using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.ViewModel.GameController;
using PickUpSports.Models.ViewModel.VenueOwnerController;

namespace PickUpSports.Models.ViewModel.VenueController
{
    public class VenueViewModel
    {
        public int VenueId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        public List<BusinessHoursViewModel> BusinessHours { get; set; }

        public decimal? AverageRating { get; set; }

        public string LatitudeCoord { get; set; }

        public string LongitudeCoord { get; set; }

        public double Distance { get; set; }

        public bool HasVenueOwner { get; set; }

        public VenueOwnerViewModel VenueOwner { get; set; }
    }
}