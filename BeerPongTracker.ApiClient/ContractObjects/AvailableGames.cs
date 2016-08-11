namespace BeerPongTracker.ApiClient.ContractObjects
{
    using System.Collections.Generic;

    public class AvailableGames
    {
        public IEnumerable<AvailableGameData> AvailableGameDatas { get; set; }
    }
}