using System.Collections.Generic;
using PickUpSports.Models.ViewModel.GameController;

namespace PickUpSports.Models.ViewModel.ContactController
{
    public class GameProfileViewModel
    {
        public int ContactId { get; set; }

        public bool IsPublicProfileView { get; set; }

        public List<GameListViewModel> Games { get; set; }
    }
}