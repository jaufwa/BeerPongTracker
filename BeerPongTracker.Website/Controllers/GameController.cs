using System.Collections.Generic;
using System.Web.Mvc;
using BeerPongTracker.Website.Models;

namespace BeerPongTracker.Website.Controllers
{
    using System.Configuration;

    using BeerPongTracker.ApiClient.Client;
    using BeerPongTracker.Website.ViewBuilders;
    using Mocks;
    public class GameController : Controller
    {
        public ActionResult TwoPlayer()
        {
            var numberOfCups = 15;

            var player1 = new TeamStatsViewModel()
            {
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

            var viewModel = new GameViewModel()
            {
                TeamStatsViewModels = new Dictionary<int, TeamStatsViewModel>()
                {
                    {1, player1},
                    {2, player2 }
                },
                TableCupCoversViewModel = tableCupCoversViewModel,
            };

            return View(viewModel);
        }

        public ActionResult ThreePlayer()
        {
            var numberOfCups = 15;

            var player1 = new TeamStatsViewModel()
            {
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
                AvatarViewModel = new AvatarViewModel()
                {
                    FacebookId = "666875244"
                },
                TeamNameViewModel = new TeamNameViewModel()
                {
                    Name = "Tucker"
                }
            };

            var player3 = new TeamStatsViewModel()
            {
                AvatarViewModel = new AvatarViewModel()
                {
                    FacebookId = "682905112"
                },
                TeamNameViewModel = new TeamNameViewModel()
                {
                    Name = "Danny"
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

            var team3CupCover = new CupCoverViewModel()
            {
                NumberOfCups = numberOfCups,
                TeamId = 3
            };

            var tableCupCoversViewModel = new TableCupCoversViewModel()
            {
                TeamCupCovers = new Dictionary<int, CupCoverViewModel>()
                {
                    {1, team1CupCover},
                    {2, team2CupCover},
                    {3, team3CupCover},
                }
            };

            var viewModel = new GameViewModel()
            {
                TeamStatsViewModels = new Dictionary<int, TeamStatsViewModel>()
                {
                    {1, player1 },
                    {2, player2 },
                    {3, player3 }
                },
                TableCupCoversViewModel = tableCupCoversViewModel,
            };

            return View(viewModel);
        }

        public ActionResult FourPlayer()
        {
            var numberOfCups = 15;

            var player1 = new TeamStatsViewModel()
            {
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
                AvatarViewModel = new AvatarViewModel()
                {
                    FacebookId = "666875244"
                },
                TeamNameViewModel = new TeamNameViewModel()
                {
                    Name = "Tucker"
                }
            };

            var player3 = new TeamStatsViewModel()
            {
                AvatarViewModel = new AvatarViewModel()
                {
                    FacebookId = "682905112"
                },
                TeamNameViewModel = new TeamNameViewModel()
                {
                    Name = "Danny"
                }
            };

            var player4 = new TeamStatsViewModel()
            {
                AvatarViewModel = new AvatarViewModel()
                {
                    FacebookId = "839635028"
                },
                TeamNameViewModel = new TeamNameViewModel()
                {
                    Name = "Karl"
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

            var team3CupCover = new CupCoverViewModel()
            {
                NumberOfCups = numberOfCups,
                TeamId = 3
            };

            var team4CupCover = new CupCoverViewModel()
            {
                NumberOfCups = numberOfCups,
                TeamId = 4
            };

            var tableCupCoversViewModel = new TableCupCoversViewModel()
            {
                TeamCupCovers = new Dictionary<int, CupCoverViewModel>()
                {
                    {1, team1CupCover},
                    {2, team2CupCover},
                    {3, team3CupCover},
                    {4, team4CupCover},
                }
            };

            var viewModel = new GameViewModel()
            {
                TeamStatsViewModels = new Dictionary<int, TeamStatsViewModel>()
                {
                    {1, player1 },
                    {2, player2 },
                    {3, player3 },
                    {4, player4 },
                },
                TableCupCoversViewModel = tableCupCoversViewModel,
            };

            return View(viewModel);
        }

        public ActionResult Build(int gameId)
        {
            //var gameViewBuilder = new GameViewBuilder(new BeerBongTrackerApiClient(ConfigurationManager.AppSettings["ApiUrl"]));

            var gameViewBuilder = new GameViewBuilder(new MockBeerBongTrackerApiClient());

            var viewHelper = gameViewBuilder.Build(gameId);

            return View(viewHelper.ViewPath, viewHelper.ViewModel);
        }
    }
}