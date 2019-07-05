using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly PickUpContext _context;

        public VenueRepository(PickUpContext context)
        {
            _context = context;
        }

        public Venue AddVenue(Venue venue)
        {
            _context.Venues.Add(venue);
            _context.SaveChanges();

            return venue;
        }

        public Venue GetVenueById(int id)
        {
            Venue venue = _context.Venues.Find(id);
            if (venue == null) return null;
            return venue;
        }

        public void Edit(Venue venue)
        {
            _context.Entry(venue).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Venue venue)
        {
            Venue existing = _context.Venues.Find(venue.VenueId);
            if (existing == null) throw new ArgumentNullException($"Could not find existing venue by ID {venue.VenueId}");

            _context.Venues.Remove(existing);
            _context.SaveChanges();
        }

        public List<Venue> GetAllVenues()
        {
            return _context.Venues.ToList();
        }
    }
}