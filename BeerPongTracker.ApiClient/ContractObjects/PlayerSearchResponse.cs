using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.ApiClient.ContractObjects
{
    public class PlayerSearchResponse
    {
        public IEnumerable<PlayerSearchResult> PlayerSearchResults { get; set; }
    }
}
