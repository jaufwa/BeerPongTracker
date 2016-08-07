using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.Core
{
    public class StringHelper
    {
        public static string ShortenName(string name)
        {
            var nameTokens = name.Split(' ');

            if (nameTokens.Count() == 1)
            {
                return nameTokens[0];
            }

            var firstName = nameTokens[0];
            var lastIniitial = nameTokens[1].Substring(0, 1);

            return $"{firstName} {lastIniitial}";
        }
    }
}
