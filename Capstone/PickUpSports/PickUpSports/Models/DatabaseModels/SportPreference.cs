using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.DatabaseModels
{
    public class SportPreference
    {
        public int SportPrefID { get; set; }
        public int ContactID { get; set; }
        public int SportID { get; set; }
    }
}