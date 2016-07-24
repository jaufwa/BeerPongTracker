using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class StartGameTeam
    {
        public int? TeamId { get; set; }
        public IEnumerable<StartGamePlayer> Players { get; set; }
    }
}
