using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IGameStatusRepository
    {
        List<GameStatus> GetAllGameStatuses();
    }
}