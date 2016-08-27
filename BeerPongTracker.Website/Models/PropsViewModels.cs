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

        public bool Active { get; set; }

        public CupViewModel(int teamId, int cupId, bool active)
        {
            TeamId = teamId;
            CupId = cupId;
            Active = active;
        }
    }

    public class CupCoverViewModel
    {
        public int GameId { get; set; }

        public int TeamId { get; set; }

        public int NumberOfCups { get; set; }

        public ICollection<CupViewModel> Cups { get; set; }
    }

    public class TableCupCoversViewModel
    {
        public Dictionary<int, CupCoverViewModel> TeamCupCovers { get; set; }

        public bool Controlling { get; set; }

        public bool IsPc { get; set; }

        public bool RemoteView => Controlling & !IsPc;
    }
}