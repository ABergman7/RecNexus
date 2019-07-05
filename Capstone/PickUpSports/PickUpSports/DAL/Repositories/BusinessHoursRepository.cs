using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class BusinessHoursRepository : IBusinessHoursRepository
    {
        private readonly PickUpContext _context;

        public BusinessHoursRepository(PickUpContext context)
        {
            _context = context;
        }

        public BusinessHours AddBusinessHours(BusinessHours businessHours)
        {
            _context.BusinessHours.Add(businessHours);
            _context.SaveChanges();

            return businessHours;
        }

        public BusinessHours GetBusinessHours(int id)
        {
            BusinessHours businessHours = _context.BusinessHours.Find(id);
            if (businessHours == null) return null;
            return businessHours;
        }

        public void Edit(BusinessHours businessHours)
        {
            _context.Entry(businessHours).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(BusinessHours businessHours)
        {
            BusinessHours existing = _context.BusinessHours.Find(businessHours.BusinessHoursId);
            if (existing == null) throw new ArgumentNullException($"Could not find business hours by ID {businessHours.BusinessHoursId}");

            _context.BusinessHours.Remove(existing);
            _context.SaveChanges();
        }

        public List<BusinessHours> GetAllBusinessHours()
        {
            return _context.BusinessHours.ToList();
        }
    }
}