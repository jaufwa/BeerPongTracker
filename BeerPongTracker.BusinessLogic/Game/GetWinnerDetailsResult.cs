using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class GetWinnerDetailsResult
    {
        public string YouTubeVideoId { get; set; }

        public IEnumerable<WinnerDetail> WinnerDetails { get; set; }
    }
}
