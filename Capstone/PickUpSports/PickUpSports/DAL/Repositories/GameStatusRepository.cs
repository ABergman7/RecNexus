using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class GameStatusRepository : IGameStatusRepository
    {
        private readonly PickUpContext _context;

        public GameStatusRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<GameStatus> GetAllGameStatuses()
        {
            return _context.GameStatuses.ToList();
        }
    }
}