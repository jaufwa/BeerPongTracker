using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class CupViewModel
    {
        public int TeamId { get; set; }

        public int CupId { get; set; }

        public CupViewModel(int teamId, int cupId)
        {
            TeamId = teamId;
            CupId = cupId;
        }
    }

    public class CupCoverViewModel
    {
        public int TeamId { get; set; }

        public int NumberOfCups { get; set; }
    }

    public class TableCupCoversViewModel
    {
        public Dictionary<int, CupCoverViewModel> TeamCupCovers { get; set; }
    }
}