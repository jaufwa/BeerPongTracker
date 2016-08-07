using BeerPongTracker.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerPongTracker.BusinessLogic.Game;

namespace BeerPongTracker.BusinessLogic.Cup
{
    public class CupLogic : ICupLogic
    {
        private readonly BeerPongFederationEntities _beerPongFederationEntities;

        public CupLogic(BeerPongFederationEntities beerPongFederationEntities)
        {
            _beerPongFederationEntities = beerPongFederationEntities;
        }

        public Game.Game CupSwitch(CupSwitchRequest cupSwitchRequest)
        {
            throw new NotImplementedException();
        }
    }
}
