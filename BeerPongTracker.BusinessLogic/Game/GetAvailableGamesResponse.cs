﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class GetAvailableGamesResponse
    {
        public IEnumerable<AvailableGameData> AvailableGames { get; set; }
    }
}
