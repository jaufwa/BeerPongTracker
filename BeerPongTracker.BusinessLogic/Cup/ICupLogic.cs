using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerPongTracker.BusinessLogic.Game;

namespace BeerPongTracker.BusinessLogic.Cup
{
    public interface ICupLogic
    {
        Game.Game CupSwitch(CupSwitchRequest cupSwitchRequest);
    }
}
