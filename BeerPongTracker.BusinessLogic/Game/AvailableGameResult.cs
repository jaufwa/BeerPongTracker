using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class AvailableGameResult
    {
        public int GameId { get; set; }

        public DateTime DateStarted { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; }
    }
}
