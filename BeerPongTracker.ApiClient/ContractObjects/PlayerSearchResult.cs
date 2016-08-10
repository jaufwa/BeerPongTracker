using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.ApiClient.ContractObjects
{
    public class PlayerSearchResult
    {
        public int PlayerId { get; set; }

        public string Name { get; set; }

        public string FacebookId { get; set; }
    }
}
