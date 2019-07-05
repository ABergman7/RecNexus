using System.Collections.Generic;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Services
{
    public class VenueOwnerServices : IVenueOwnerService
    {
        private readonly IVenueOwnerRepository _venueOwnerRepository;

        public VenueOwnerServices(IVenueOwnerRepository venueOwnerRepository)
        {
            _venueOwnerRepository = venueOwnerRepository;
        }

        /*
         * Given a new Venue Owner, add it to the database
         */
        public VenueOwner AddVenueOwner(VenueOwner owner)
        {
            return _venueOwnerRepository.AddVenueOwner(owner);
        }

        /*
         * Given an existing, modified Venue Owner, modify that entity in the database
         */
        public void EditVenueOwner(VenueOwner owner)
        {
            _venueOwnerRepository.Edit(owner);
        }

        /*
         * Given an email, return that Venue Owner 
         */
        public VenueOwner GetVenueOwnerByEmail(string email)
        {
            return _venueOwnerRepository.GetVenueOwnerByEmail(email);
        }

        /*
         * Returns a list of all Venue Owners
         */
        public List<VenueOwner> GetAllVenueOwners()
        {
            return _venueOwnerRepository.GetAllVenueOwners();
        }

        /*
         * Given a Venue Owner ID, return that Venue Owner 
         */
        public VenueOwner GetVenueOwnerById(int venueOwnerId)
        {
            return _venueOwnerRepository.GetVenueOwnerById(venueOwnerId);
        }

        /*
         * Given a Venue ID, return Owner of that Venue if one exists
         */
        public VenueOwner GetVenueOwnerByVenueId(int venueId)
        {
            return _venueOwnerRepository.GetVenueOwnerByVenueId(venueId);
        }

        /*
         * Given an email, check if that user is a venue owner
         */
        public bool IsVenueOwner(string email)
        {
            VenueOwner owner = _venueOwnerRepository.GetVenueOwnerByEmail(email);

            if (owner == null) return false;

            return true;
        }

        /*
         * Given a Venue, check if that Venue currently has a VenueOwner
         */
        public bool VenueHasOwner(Venue venue)
        {
            var existingVenueOwner = _venueOwnerRepository.GetVenueOwnerByVenueId(venue.VenueId);

            if (existingVenueOwner == null) return false;

            return true;
        }
    }
}