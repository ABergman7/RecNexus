using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel
{
    public class MessagessViewModel
    {

        public int FriendId { get; set; }
        public int ContactId { get; set; }

        public int ContactFriendId { get; set; }

        public string FriendName { get; set; }

        public string FriendEmail { get; set; }

        public string FriendNumber { get; set; }


    }
}