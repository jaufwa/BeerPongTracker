using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class TwoPlayerViewModel
    {
        public TeamStatsViewModel Team1Stats { get; set; }

        public TeamStatsViewModel Team2Stats { get; set; }

        public TableCupCoversViewModel TwoPlayerTableViewModel { get; set; }
    }
}