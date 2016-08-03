using BeerPongTracker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Cup
{
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

                    int healthPercentage = BeerPongMath.GetPercentage(activeCupsCount, totalCupsCount);

                    teamHealth.Add(teamId, healthPercentage);
                }

                return teamHealth;
            }
        }

        public IEnumerable<CupStats> CupStats { get; set; }
    }
}
