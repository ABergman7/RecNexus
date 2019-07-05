using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
    public interface IVenueOwnerService
    {
        List<VenueOwner> GetAllVenueOwners();

        VenueOwner GetVenueOwnerById(int venueOwnerId);

        VenueOwner GetVenueOwnerByEmail(string email);

        VenueOwner GetVenueOwnerByVenueId(int venueId);

        bool VenueHasOwner(Venue venue);

        VenueOwner AddVenueOwner(VenueOwner owner);

        void EditVenueOwner(VenueOwner owner);

        bool IsVenueOwner(string email);
    }
}