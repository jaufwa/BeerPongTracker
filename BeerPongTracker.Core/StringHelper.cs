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

        public static string CombinePlayerNames(IEnumerable<string> names)
        {
            var namesAsInitials = new List<string>();

            foreach (var name in names)
            {
                var tokens = name.Split(' ');

                var initials = "";

                foreach (var token in tokens)
                {
                    initials += token.Substring(0, 1).ToUpper();
                }

                namesAsInitials.Add(initials);
            }

            return string.Join(" & ", namesAsInitials);
        }

        public static string CombinePlayerNamesProper(IEnumerable<string> names)
        {
            var sb = new StringBuilder();

            var i = 0;
            foreach(var name in names)
            {
                i++;

                if (i == names.Count())
                {
                    sb.Append(name);
                } else if (i == names.Count() - 1)
                {
                    sb.Append($"{name} and ");
                } else
                {
                    sb.Append($"{name}, ");
                }
            }

            return sb.ToString();
        }
    }
}
