using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.ApiClient.ContractObjects
{
    public class ListenForChangeRequest
    {
        public int GameId { get; set; }

        public string LastUpdateSignature { get; set; }
    }
}
