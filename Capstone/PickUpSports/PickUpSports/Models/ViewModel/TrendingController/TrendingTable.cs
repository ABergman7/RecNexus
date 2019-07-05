using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PickUpSports.Models.ViewModel.TrendingController
{
    public class TrendingTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
}