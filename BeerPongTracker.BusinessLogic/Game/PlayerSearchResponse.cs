using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class PlayerSearchResponse
    {
        public IEnumerable<PlayerSearchResult> PlayerSearchResults { get; set; }
    }
}
