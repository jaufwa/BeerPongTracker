namespace BeerPongTracker.BusinessLogic.Game
{
    using System.Collections.Generic;

    using BeerPongTracker.BusinessLogic.Cup;

    public class Team
    {
        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public string FacebookId { get; set; }

        public int Health { get; set; }

        public ICollection<CupStats> CupStats { get; set; }
    }
}