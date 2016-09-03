using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.ApiClient.ContractObjects
{
    public class DeclareWinnerRequest
    {
        public int WinningTeamId { get; set; }

        public int GameId { get; set; }

        public bool Controlling { get; set; }
    }
}