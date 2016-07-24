using BeerPongTracker.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Cup
{
    public class CupLogic : ICupLogic
    {
        private readonly BeerPongFederationEntities _beerPongFederationEntities;

        public CupLogic(BeerPongFederationEntities beerPongFederationEntities)
        {
            _beerPongFederationEntities = beerPongFederationEntities;
        }

        public CupSwitchResponse CupSwitch(CupSwitchRequest cupSwitchRequest)
        {
            var cupTracker = _beerPongFederationEntities.CupTracker.FirstOrDefault(x => 
                x.GameId == cupSwitchRequest.GameId && 
                x.TeamId == cupSwitchRequest.TeamId && 
                x.CupId == cupSwitchRequest.CupId);

            if (cupTracker == null)
            {
                return new CupSwitchResponse();
            }

            cupTracker.Active = !cupTracker.Active;

            _beerPongFederationEntities.SaveChanges();

            var cupSwitchResponse = new CupSwitchResponse();

            cupSwitchResponse.GameId = cupSwitchRequest.GameId;

            var dbCupStats = _beerPongFederationEntities.CupTracker.Where(x => x.GameId == cupSwitchRequest.GameId).ToList();

            var cupStats = new List<CupStats>();

            foreach (var dbCupStat in dbCupStats)
            {
                cupStats.Add(
                    new CupStats()
                    {
                        TeamId = dbCupStat.TeamId,
                        CupId = dbCupStat.CupId,
                        Active = dbCupStat.Active
                    }
                );
            }

            cupSwitchResponse.CupStats = cupStats;

            return cupSwitchResponse;
        }
    }
}
