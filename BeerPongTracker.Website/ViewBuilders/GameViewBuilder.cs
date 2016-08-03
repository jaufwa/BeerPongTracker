namespace BeerPongTracker.Website.ViewBuilders
{
    using System.Collections.Generic;

    using BeerPongTracker.ApiClient.Client;
    using BeerPongTracker.ApiClient.ContractObjects;
    using BeerPongTracker.Website.Models;

    public class GameViewBuilder
    {
        private readonly IBeerBongTrackerApiClient _beerBongTrackerApiClient;

        public GameViewBuilder(IBeerBongTrackerApiClient beerBongTrackerApiClient)
        {
            _beerBongTrackerApiClient = beerBongTrackerApiClient;
        }

        public ViewBuilderResult Build(int gameId)
        {
            var gameState = _beerBongTrackerApiClient.Get<Game>($"Game/{gameId}");

            var teamStatsViewModels = new Dictionary<int, TeamStatsViewModel>();

            var teamCupCovers = new Dictionary<int, CupCoverViewModel>(); 

            foreach (var team in gameState.Teams)
            {
                var teamStatsViewModel = new TeamStatsViewModel
                {
                    AvatarViewModel = new AvatarViewModel
                    {
                        FacebookId = team.FacebookId
                    },
                    TeamNameViewModel = new TeamNameViewModel
                    {
                        Name = team.TeamName
                    }
                };

                teamStatsViewModels.Add(team.TeamId, teamStatsViewModel);

                var cupCoverViewModel = new CupCoverViewModel
                {
                    NumberOfCups = gameState.NumberOfCups,
                    TeamId = team.TeamId
                };

                teamCupCovers.Add(team.TeamId, cupCoverViewModel);
            }

            var tableCupCoversViewModel = new TableCupCoversViewModel
            {
                TeamCupCovers = teamCupCovers
            };

            var viewModel = new GameViewModel
            {
                TableCupCoversViewModel = tableCupCoversViewModel,
                TeamStatsViewModels = teamStatsViewModels
            };

            var viewPathMap = new Dictionary<int, string>()
            {
                {2, "Game/_TwoPlayer" },
                {3, "Game/_ThreePlayer" },
                {4, "Game/_FourPlayer" }
            };

            var viewPath = viewPathMap[gameState.NumberOfTeams];

            return new ViewBuilderResult(viewPath, viewModel);
        }
    }
}