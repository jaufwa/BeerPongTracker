namespace BeerPongTracker.ApiClient.ContractObjects
{
    using System.Collections.Generic;

    public class StartGameTeam
    {
        public int? TeamId { get; set; }
        public IEnumerable<StartGamePlayer> Players { get; set; }
    }
}
