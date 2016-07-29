namespace BeerPongTracker.ApiClient.ContractObjects
{
    using System.Collections.Generic;

    public class Team
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public string FacebookId { get; set; }

        public int Health { get; set; }

        public ICollection<CupStats> CupStats { get; set; }
    }
}