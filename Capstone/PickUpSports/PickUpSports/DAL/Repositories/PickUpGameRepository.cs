using System;
using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.DAL.Repositories
{
    public class PickUpGameRepository : IPickUpGameRepository
    {
        private readonly PickUpContext _context;

        public PickUpGameRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<PickUpGame> GetPickUpGameListByGameId(int gameId)
        { 
            List<PickUpGame> pickUpGameList = _context.PickUpGames.Where(x => x.GameId == gameId).ToList();
            if (pickUpGameList.Count <= 0) return null;

            return pickUpGameList;
        }

        public List<PickUpGame> GetPickUpGameListByContactId(int contactId)
        {
            List<PickUpGame> pickUpGameList = _context.PickUpGames.Where(x => x.ContactId == contactId).ToList();
            if (pickUpGameList.Count <= 0) return null;

            return pickUpGameList;
        }

        public PickUpGame AddPickUpGame(PickUpGame pickUpGame)
        {
            _context.PickUpGames.Add(pickUpGame);
            _context.SaveChanges();
            return pickUpGame;
        }

        public void DeletePickUpGame(PickUpGame pickUpGame)
        {
            PickUpGame existing = _context.PickUpGames.Find(pickUpGame.PickUpGameId);
            if (existing == null) throw new ArgumentNullException($"Could not find existing pick up game by ID {pickUpGame.PickUpGameId}");

            _context.PickUpGames.Remove(existing);
            _context.SaveChanges();
        }
    }
}