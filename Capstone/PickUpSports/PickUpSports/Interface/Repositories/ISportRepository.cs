using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface ISportRepository
    {
        Sport GetSportById(int id);

        List<Sport> GetAllSports();
    }
}