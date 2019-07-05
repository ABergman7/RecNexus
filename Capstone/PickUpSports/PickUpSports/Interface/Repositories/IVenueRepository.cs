using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IVenueRepository
    {
        Venue AddVenue(Venue venue);

        Venue GetVenueById(int id);

        void Edit(Venue venue);

        void Delete(Venue venue);

        List<Venue> GetAllVenues();
    }
}