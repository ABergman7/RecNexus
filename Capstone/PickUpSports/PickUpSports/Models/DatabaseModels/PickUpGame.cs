using System.ComponentModel.DataAnnotations;

namespace PickUpSports.Models.DatabaseModels
{
    public class PickUpGame
    {
        public int PickUpGameId { get; set; }

        public int GameId { get; set; }

        public int ContactId { get; set; }
    }
}