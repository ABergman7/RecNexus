using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class VenueOwnerRepository : IVenueOwnerRepository
    {
        private readonly PickUpContext _context;

        public VenueOwnerRepository(PickUpContext context)
        {
            _context = context;
        }

        public VenueOwner AddVenueOwner(VenueOwner venueOwner)
        {
            _context.VenueOwners.Add(venueOwner);
            _context.SaveChanges();

            return venueOwner;
        }

        public VenueOwner GetVenueOwnerById(int id)
        {
            VenueOwner venueOwner = _context.VenueOwners.Find(id);
            if (venueOwner == null) return null;
            return venueOwner;
        }

        public void Edit(VenueOwner venueOwner)
        {
            _context.Entry(venueOwner).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(VenueOwner venueOwner)
        {
            VenueOwner existing = _context.VenueOwners.Find(venueOwner.VenueOwnerId);
            if (existing == null) throw new ArgumentNullException($"Could not find existing venue owner by ID {venueOwner.VenueOwnerId}");

            _context.VenueOwners.Remove(existing);
            _context.SaveChanges();
        }

        public List<VenueOwner> GetAllVenueOwners()
        {
           return _context.VenueOwners.ToList();
        }

        public VenueOwner GetVenueOwnerByEmail(string email)
        {
            var owner = _context.VenueOwners.FirstOrDefault(x => x.Email == email);

            if (owner == null) return null;

            return owner;
        }

        public VenueOwner GetVenueOwnerByVenueId(int venueId)
        {
            var owner = _context.VenueOwners.FirstOrDefault(x => x.VenueId == venueId);

            if (owner == null) return null;

            return owner;
        }
    }
}