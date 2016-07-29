namespace BeerPongTracker.ApiClient.ContractObjects
{
    using System.Collections.Generic;

    public class StartGameRequest
    {
        public IEnumerable<StartGameTeam> Teams { get; set; }

        public int NumberOfCups { get; set; }
    }
}
