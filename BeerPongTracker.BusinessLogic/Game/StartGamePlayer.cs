using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class StartGamePlayer
    {
        public int? PlayerId { get; set; }

        public bool NewPlayer => PlayerId.HasValue == false;

        public string Name { get; set; }

        public int? TeamId { get; set; }
    }
}
