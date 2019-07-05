using System;
using System.Collections.Generic;
using System.Data.Entity;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using System.Linq;

namespace PickUpSports.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly PickUpContext _context;

        public GameRepository(PickUpContext context)
        {
            _context = context;
        }

        public List<Game> GetAllGames()
        {
            return _context.Games.ToList();
        }

        public Game AddGame(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
            return game;
        }

        public Game GetGameById(int id)
        {
            return _context.Games.Find(id);
        }

        public void EditGame(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteGame(Game game)
        {
            Game existing = _context.Games.Find(game.GameId);
            if (existing == null) throw new ArgumentNullException($"Could not find existing game by ID {game.GameId}");

            _context.Games.Remove(existing);
            _context.SaveChanges();
        }

        public void RejectGame(Game game)
        {
            game.GameStatusId = 4;
            _context.SaveChanges();
        }

        public void AcceptGame(Game game)
        {
            game.GameStatusId = 3;
            _context.SaveChanges();
        }
    }
}