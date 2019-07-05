using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class SportPreferenceRepository : ISportPreferenceRepository
    {
        private readonly PickUpContext _context;

        public SportPreferenceRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<SportPreference> GetAllSportsPreferences()
        {
            return _context.SportPreferences.ToList();
        }

        public void Delete(SportPreference sportPreference)
        {
            _context.SportPreferences.Remove(sportPreference);
            _context.SaveChanges();
        }
    }
}