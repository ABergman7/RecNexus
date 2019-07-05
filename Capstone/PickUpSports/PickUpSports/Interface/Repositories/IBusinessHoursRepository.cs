using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IBusinessHoursRepository
    {
        BusinessHours AddBusinessHours(BusinessHours businessHours);

        BusinessHours GetBusinessHours(int id);

        void Edit(BusinessHours businessHours);

        void Delete(BusinessHours businessHours);

        List<BusinessHours> GetAllBusinessHours();
    }
}