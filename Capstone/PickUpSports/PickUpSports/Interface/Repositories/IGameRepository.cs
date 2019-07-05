using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IGameRepository
    {
        List<Game> GetAllGames();

        Game AddGame(Game game);

        Game GetGameById(int id);

        void EditGame(Game game);

        void DeleteGame(Game game);

        void RejectGame(Game game);

        void AcceptGame(Game game);
    }
}