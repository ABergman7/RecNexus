using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PickUpSports.Models.ViewModel
{
    public class TrendingModel
    {
        public string VenueName { get; set; }
        public string SportName { get; set; }
        
       

        public override string ToString()
        {
            return VenueName + "," + SportName;
        }
    }
}