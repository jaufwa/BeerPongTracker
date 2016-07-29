namespace BeerPongTracker.ApiClient.ContractObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CupSwitchResponse
    {
        public int GameId { get; set; }

        public Dictionary<int, int> TeamHealth
        {
            get
            {
                var teamHealth = new Dictionary<int, int>();

                foreach (var teamId in CupStats.Select(x => x.TeamId).Distinct().ToArray())
                {
                    var activeCupsCount = CupStats.Where(x => x.TeamId == teamId && x.Active).Count();

                    var totalCupsCount = CupStats.Where(x => x.TeamId == teamId).Count();

                    int healthPercentage = (int)Math.Round((double)100 * (activeCupsCount) / totalCupsCount);

                    teamHealth.Add(teamId, healthPercentage);
                }

                return teamHealth;
            }
        }

        public IEnumerable<CupStats> CupStats { get; set; }
    }
}
