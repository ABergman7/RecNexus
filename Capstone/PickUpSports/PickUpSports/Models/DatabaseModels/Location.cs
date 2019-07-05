namespace PickUpSports.Models.DatabaseModels
{
    public class Location
    {
        public int LocationId { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public int VenueId { get; set; }
    }
}