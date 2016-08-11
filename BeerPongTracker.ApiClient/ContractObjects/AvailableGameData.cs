namespace BeerPongTracker.ApiClient.ContractObjects
{
    using System;

    public class AvailableGameData
    {
        public DateTime StartDate { get; set; }

        public string Hint { get; set; }

        public int GameId { get; set; }
    }
}