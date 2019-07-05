using System;
using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface
{
    public interface IGameService
    {
        Game CreateGame(Game game);

        PickUpGame AddPlayerToGame(PickUpGame pickUpGame);

        void RemovePlayerFromGame(PickUpGame pickUpGame);

        List<PickUpGame> GetPickUpGameListByGameId(int gameId);

        List<PickUpGame> GetPickUpGamesByContactId(int contactId);

        Game GetGameById(int id);

        void EditGame(Game game);

        List<Game> GetAllGamesByContactId(int contactId);

        List<Game> GetAllCurrentOpenGames();

        List<Game> GetCurrentOrderedGamesByContactId(int contactId);

        List<Game> GetCurrentGamesByVenueId(int venueId);

        List<Game> GetCurrentGamesBySportId(int sportId);

        string GetSportNameById(int sportId);

        List<Sport> GetAllSports();

        bool IsNotSignedUpForGame(int contactId, List<PickUpGame> games);

        bool IsCreatorOfGame(int contactId, Game game);

        bool IsSelectedTimeValid(DateTime startDateTime, DateTime endDataTime);

        bool IsThisGameCanCancel(DateTime dateTime);

        Game CheckForExistingGame(int venueId, int sportId, DateTime startDateTime);

        Game CheckForExistingGameExceptItself(int venueId, int sportId, DateTime startDateTime, int gameId);

        Game RejectGame(int gameId);

        Game AcceptGame(int gameId);

        List<GameStatus> GetAllGameStatuses();
    }
}