using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class ListenForChangeRequest
    {
        public int GameId { get; set; }

        public string LastUpdateSignature { get; set; }
    }
}
