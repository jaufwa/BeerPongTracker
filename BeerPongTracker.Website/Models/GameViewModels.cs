﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class GameViewModel
    {
        public Dictionary<int, TeamStatsViewModel> TeamStatsViewModels { get; set; }

        public TableCupCoversViewModel TableCupCoversViewModel { get; set; }

        public string LastUpdateSignature { get; set; }

        public bool Controlling { get; set; }

        public bool IsPc { get; set; }

        public bool RemoteView => Controlling & !IsPc;
    }
}