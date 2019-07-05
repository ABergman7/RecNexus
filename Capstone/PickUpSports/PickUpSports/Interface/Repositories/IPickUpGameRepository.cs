using System.Collections.Generic;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Interface.Repositories
{
    public interface IPickUpGameRepository
    {
        List<PickUpGame> GetPickUpGameListByGameId(int gameId);

        List<PickUpGame> GetPickUpGameListByContactId(int contactId);

        PickUpGame AddPickUpGame(PickUpGame pickUpGame);

        void DeletePickUpGame(PickUpGame pickUpGame);
    }
}