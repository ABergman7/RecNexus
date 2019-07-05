using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PickUpSports.Models.DatabaseModels;

namespace PickUpSports.Models.ViewModel
{
    public class ManageUserViewModel
    {

        public IndexViewModel Identity { get; set; }

        public Contact Contact { get; set; }
    }
}