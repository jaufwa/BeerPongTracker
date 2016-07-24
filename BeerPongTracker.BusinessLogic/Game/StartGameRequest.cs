using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class StartGameRequest
    {
        public IEnumerable<StartGameTeam> Teams { get; set; }

        public int NumberOfCups { get; set; }
    }
}
