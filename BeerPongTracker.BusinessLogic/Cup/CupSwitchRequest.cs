using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Cup
{
    public class CupSwitchRequest
    {
        public int GameId { get; set; }

        public int TeamId { get; set; }

        public int CupId { get; set; }
    }
}
