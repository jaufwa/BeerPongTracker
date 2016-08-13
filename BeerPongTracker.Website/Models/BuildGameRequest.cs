using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class BuildGameRequest
    {
        public int GameId { get; set; }

        public bool Controlling { get; set; }
    }
}