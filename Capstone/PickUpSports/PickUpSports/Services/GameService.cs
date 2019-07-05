using System;
using System.Collections.Generic;
using System.Linq;
using PickUpSports.Interface;
using PickUpSports.Interface.Repositories;
using PickUpSports.Models.DatabaseModels;
using PickUpSports.Models.Enums;

namespace PickUpSports.Services
{
    public class GameService : IGameService
    {
        private readonly IPickUpGameRepository _pickUpGameRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ISportRepository _sportRepository;
        private readonly IGameStatusRepository _gameStatusRepository;

        public GameService(IPickUpGameRepository pickUpGameRepository, 
            IGameRepository gameRepository, 
            ISportRepository sportRepository, 
            IGameStatusRepository gameStatusRepository)
        {
            _pickUpGameRepository = pickUpGameRepository;
            _gameRepository = gameRepository;
            _sportRepository = sportRepository;
            _gameStatusRepository = gameStatusRepository;
        }

        /*
         * Given a Game entity, add that game to the daabase
         */
        public Game CreateGame(Game game)
        {
            return _gameRepository.AddGame(game);
        }

        /*
         * Method to add a player to an existing game
         */
        public PickUpGame AddPlayerToGame(PickUpGame pickUpGame)
        {
            return _pickUpGameRepository.AddPickUpGame(pickUpGame);
        }

        /*
         * Method to remove a player from a game that they previously joined
         */
        public void RemovePlayerFromGame(PickUpGame pickUpGame)
        {
            _pickUpGameRepository.DeletePickUpGame(pickUpGame);
        }

        /*
         * Given Game ID, get all pick up players for that game
         */
        public List<PickUpGame> GetPickUpGameListByGameId(int gameId)
        {
            var results = _pickUpGameRepository.GetPickUpGameListByGameId(gameId);
            return results;
        }

        /*
         * Given Contact ID, get all games that player is signed up for
         */
        public List<PickUpGame> GetPickUpGamesByContactId(int contactId)
        {
            var results = _pickUpGameRepository.GetPickUpGameListByContactId(contactId);
            if (results == null) return null;
            return results;
        }

        /*
         * Given game, edit that game
         */
        public void EditGame(Game game)
        {
            _gameRepository.EditGame(game);
        }

        /*
         * Given Contact ID, get all games that player started 
         */
        public List<Game> GetAllGamesByContactId(int contactId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;
 
            var results = from g in games
                where g.ContactId.Equals(contactId)
                select g;

            if (!results.Any()) return null;
            return results.ToList();
        }

        /*
         * Get all games that are open that haven't passed yet.
         * Order by start time
         */
        public List<Game> GetAllCurrentOpenGames()
        {
            var games = _gameRepository.GetAllGames();
            var results = games.Where(g => g.GameStatusId == (int)GameStatusEnum.Open 
                                           && g.StartTime > DateTime.Now)
                .OrderBy(g => g.StartTime).ToList();

            return results;
        }

        /*
         * Given a GameID, return that game
         */
        public Game GetGameById(int id)
        {
            var result = _gameRepository.GetGameById(id);
            return result;
        }
        
        /*
         * Given ContactID, returns list of current games
         * in ascending order by the ending time of the game
         */
        public List<Game> GetCurrentOrderedGamesByContactId(int contactId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;

            var results = from g in games
                where g.ContactId == contactId &&
                      g.EndTime > DateTime.Today.AddDays(-1)
                      orderby g.StartTime  
                select g;

            if (!results.Any()) return null;
            return results.ToList();
        }

        /*
         * Given VenueID, returns list of games
         * that haven't passed yet 
         */
        public List<Game> GetCurrentGamesByVenueId(int venueId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;

            var results = games.Where(x => x.VenueId == venueId && x.EndTime > DateTime.Today.AddDays(-1)).ToList();

            if (!results.Any()) return null;
            return results.ToList();
        }

        /*
         * Given SportID, returns list of current games
         */
        public List<Game> GetCurrentGamesBySportId(int sportId)
        {
            var games = _gameRepository.GetAllGames();
            if (games == null) return null;

            var results = games.Where(x => x.SportId == sportId && x.EndTime > DateTime.Today.AddDays(-1)).ToList();

            if (!results.Any()) return null;
            return results.ToList();
        }

        /*
         * Given a Sport ID, return name of sport
         */
        public string GetSportNameById(int sportId)
        {
            var sport = _sportRepository.GetSportById(sportId);
            return sport.SportName;
        }

        /*
         * Get list of all sports
         */
        public List<Sport> GetAllSports()
        {
            return _sportRepository.GetAllSports();
        }

        /*
         * Helper method to see if user is already signed up for a game or not
         */
        public bool IsNotSignedUpForGame(int contactId, List<PickUpGame> games)
        {
            //Just in case a null "0" comes in stop it from coming in
            if (contactId == 0) return false;

            if (games == null) return true;

            foreach (var game in games)
            {
                if (game.ContactId == contactId)
                {
                    return false;
                }

            }

            // else this person hasn't signed up yet
            return true;
        }

        /*
         * Given a Contact ID and a game, check if user is
         * creater of that game
         */
        public bool IsCreatorOfGame(int contactId, Game game)
        {
            //if blank value then return false
            if (contactId == 0 || game.ContactId == 0) return false;

            if (game == null) return false;

            //see if it matches, if so then the contact is the creator
            if (contactId == game.ContactId)
            {
                return true;
            }

            return false;
        }

        /*
         * Check if given time is valid
         */
        public bool IsSelectedTimeValid(DateTime startDateTime, DateTime endDataTime)
        {
            if (startDateTime == null || endDataTime == null)
            {
                return false;
            }
            if (endDataTime.Date != startDateTime.Date)
            {
                return false;
            }

            return true;
        }

        /*
         * Games can only be cancelled if not within an hour
         * before start time
         */
        public bool IsThisGameCanCancel(DateTime dateTime)
        {
            if (dateTime.AddHours(-1) < DateTime.Now)
            {
                return false;
            }
            return true;
        }

        /*
         * Given a Venue ID, a Sport ID, and a start time, check if
         * this game already exists in the database
         */
        public Game CheckForExistingGame(int venueId, int sportId, DateTime startDateTime)
        {
            // Check for all games that are happening at same venue
            var allGames = GetAllCurrentOpenGames();
            var gamesAtVenue = allGames.Where(x => x.VenueId == venueId).ToList();
            if (gamesAtVenue.Count <= 0) return null;

            // Check for all games happening at that venue with same sport
            List<Game> sportsAtVenue = gamesAtVenue.Where(g => g.SportId == sportId).ToList();
            if (sportsAtVenue.Count <= 0) return null;

            // There are existing games with same sport and venue so check starting time
            foreach (var game in sportsAtVenue)
            {
                if (startDateTime >= game.StartTime && startDateTime <= game.EndTime)
                {
                    // If we get here, the new game will overlap with an existing game
                    // Check if status is Open and if so, return that game
                    if (game.GameStatusId == (int)GameStatusEnum.Open)
                    {
                        return game;
                    }
                }
            }

            return null;

        }

        // Added by Kexin, because the above method CheckForExistingGame is also used by creating games, there is no gameID to compare
        // make sure the existing game is not itself, user can save the game without any edition
        public Game CheckForExistingGameExceptItself(int venueId, int sportId, DateTime startDateTime, int gameId)
        {
            // Check for all games that are happening at same venue that aren't this game
            var allGames = GetAllCurrentOpenGames();
            var gamesAtVenue = allGames.Where(x => x.VenueId == venueId && x.GameId != gameId).ToList();
            if (gamesAtVenue.Count <= 0) return null;

            // Check for all games happening at that venue with same sport
            List<Game> sportsAtVenue = gamesAtVenue.Where(g => g.SportId == sportId).ToList();
            if (sportsAtVenue.Count <= 0) return null;

            // There are existing games with same sport and venue so check starting time
            foreach (var game in sportsAtVenue)
            {
                if (startDateTime >= game.StartTime && startDateTime <= game.EndTime)
                {
                    // If we get here, the new game will overlap with an existing game
                    // Check if status is Open and if so, return that game
                    if (game.GameStatusId == (int)GameStatusEnum.Open)
                    {
                        return game;
                    }
                }
            }

            return null;
        }

        // Given a Game ID, allow a Venue Owner to reject a game
        public Game RejectGame(int gameId)
        {
            Game game = GetGameById(gameId);
            _gameRepository.RejectGame(game);
            return game;
        }

        // Given a Game ID, allow a Venue Owner to accept a previously rejected game
        public Game AcceptGame(int gameId)
        {
            Game game = GetGameById(gameId);
            _gameRepository.AcceptGame(game);
            return game;
        }

        // Return a list of all possible game statuses
        public List<GameStatus> GetAllGameStatuses()
        {
            return _gameStatusRepository.GetAllGameStatuses();
        }
    }
}