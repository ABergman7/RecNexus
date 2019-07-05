using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface ISportPreferenceRepository
    {
        List<SportPreference> GetAllSportsPreferences();

        void Delete(SportPreference sportPreference);

    }
}