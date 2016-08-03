using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.Core
{
    public class BeerPongMath
    {
        public static int GetPercentage(int sample, int total)
        {
            return (int)Math.Round((double)100 * (sample) / total);
        }
    }
}
