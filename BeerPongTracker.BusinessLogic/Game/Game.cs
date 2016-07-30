﻿namespace BeerPongTracker.BusinessLogic.Game
{
    using System.Collections.Generic;

    public class Game
    {
        public int GameId { get; set; }

        public int NumberOfCups { get; set; }

        public int NumberOfTeams { get; set; }

        public ICollection<Team> Teams { get; set; }
    }
}