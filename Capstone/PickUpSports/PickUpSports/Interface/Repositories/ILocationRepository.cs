using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface ILocationRepository
    {
        List<Location> GetAllLocations();

        Location AddLocation(Location location);
    }
}