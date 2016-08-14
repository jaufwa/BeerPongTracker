namespace BeerPongTracker.Website.ViewBuilders
{
    using System.Collections.Generic;

    using BeerPongTracker.ApiClient.Client;
    using BeerPongTracker.ApiClient.ContractObjects;
    using BeerPongTracker.Website.Models;
    using Core;
    using System.Linq;
    public class GameViewBuilder
    {
        private readonly IBeerBongTrackerApiClient _beerBongTrackerApiClient;

        public GameViewBuilder(IBeerBongTrackerApiClient beerBongTrackerApiClient)
        {
            _beerBongTrackerApiClient = beerBongTrackerApiClient;
        }

        public ViewBuilderResult Build(int gameId, bool controlling)
        {
            var gameState = _beerBongTrackerApiClient.GetGame(gameId);

            var teamStatsViewModels = new Dictionary<int, TeamStatsViewModel>();

            var teamCupCovers = new Dictionary<int, CupCoverViewModel>(); 

            foreach (var team in gameState.Teams)
            {
                var teamStatsViewModel = new TeamStatsViewModel
                {
                    TeamId = team.TeamId,
                    AvatarViewModel = new AvatarViewModel
                    {
                        FacebookId = team.FacebookId
                    },
                    TeamNameViewModel = new TeamNameViewModel
                    {
                        Name = team.TeamName
                    },
                    HealthViewModel = new HealthViewModel()
                    {
                        TeamId = team.TeamId,
                        HealthPercentage = team.Health
                    }
                };

                teamStatsViewModels.Add(team.TeamId, teamStatsViewModel);

                var cupCoverViewModel = new CupCoverViewModel
                {
                    NumberOfCups = gameState.NumberOfCups,
                    TeamId = team.TeamId,
                    GameId= gameId
                };

                cupCoverViewModel.Cups = new List<CupViewModel>();

                foreach (var cup in team.CupStats)
                {
                    cupCoverViewModel.Cups.Add(new CupViewModel(team.TeamId, cup.CupId, cup.Active));
                }

                teamCupCovers.Add(team.TeamId, cupCoverViewModel);
            }

            var tableCupCoversViewModel = new TableCupCoversViewModel
            {
                TeamCupCovers = teamCupCovers,
                Controlling = controlling
            };

            var viewModel = new GameViewModel
            {
                TableCupCoversViewModel = tableCupCoversViewModel,
                TeamStatsViewModels = teamStatsViewModels,
                LastUpdateSignature = gameState.LastUpdateSignature,
                Controlling = controlling
            };

            var viewPathMap = new Dictionary<int, string>()
            {
                {2, "_TwoPlayer" },
                {3, "_ThreePlayer" },
                {4, "_FourPlayer" }
            };

            var viewPath = viewPathMap[gameState.NumberOfTeams];

            return new ViewBuilderResult(viewPath, viewModel);
        }

        public WinnerScreenViewModel BuildWinnerScreenViewModel(DeclareWinnerRequest request)
        {
            var apiResult = _beerBongTrackerApiClient.GetWinnerDetails(request);

            var viewModel = new WinnerScreenViewModel();

            viewModel.NamePlateViewModel = new WinnerNamePlateViewModel();
            viewModel.NamePlateViewModel.TeamName = StringHelper.CombinePlayerNamesProper(apiResult.WinnerDetails.Select(x => x.Name).ToList());
            viewModel.NamePlateViewModel.Plural = apiResult.WinnerDetails.Count() > 1;

            viewModel.WinnerPhotosViewModel = new WinnerPhotosViewModel();

            var photos = new List<WinnerPhotoViewModel>();

            foreach (var winner in apiResult.WinnerDetails)
            {
                photos.Add(new WinnerPhotoViewModel() { FacebookId = winner.FacebookId });
            }

            viewModel.WinnerPhotosViewModel.Photos = photos;

            return viewModel;
        }
    }
}