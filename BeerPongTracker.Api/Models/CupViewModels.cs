using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Api.Models
{
    public class CupSwitchViewModel
    {
        public int GameId { get; set; }

        public int TeamId { get; set; }

        public int CupId { get; set; }
    }
}