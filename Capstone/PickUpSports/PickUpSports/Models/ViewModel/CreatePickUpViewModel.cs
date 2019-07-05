using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel
{
    public class CreatePickUpViewModel
    {
        public int PickUpGameId { get; set; }

        public int GameId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}