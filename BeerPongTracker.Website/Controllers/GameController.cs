using BeerPongTracker.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BeerPongTracker.Website.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult TwoPlayer()
        {
            var numberOfCups = 15;

            var player1 = new TeamStatsViewModel()
            {
                Alignment = "left",
                AvatarViewModel = new AvatarViewModel()
                {
                    FacebookId = "548140192"
                },
                TeamNameViewModel = new TeamNameViewModel()
                {
                    Name = "Jonny"
                }
            };

            var player2 = new TeamStatsViewModel()
            {
                Alignment = "right",
                AvatarViewModel = new AvatarViewModel()
                {
                    FacebookId = "666875244"
                },
                TeamNameViewModel = new TeamNameViewModel()
                {
                    Name = "Tucker"
                }
            };

            var team1CupCover = new CupCoverViewModel()
            {
                NumberOfCups = numberOfCups,
                TeamId = 1
            };

            var team2CupCover = new CupCoverViewModel()
            {
                NumberOfCups = numberOfCups,
                TeamId = 2
            };

            var tableCupCoversViewModel = new TableCupCoversViewModel()
            {
                TeamCupCovers = new Dictionary<int, CupCoverViewModel>()
                {
                    {1, team1CupCover},
                    {2, team2CupCover}
                }
            };

            var viewModel = new TwoPlayerViewModel()
            {
                Team1Stats = player1,
                Team2Stats = player2,
                TwoPlayerTableViewModel = tableCupCoversViewModel,
            };

            return View(viewModel);
        }
    }
}