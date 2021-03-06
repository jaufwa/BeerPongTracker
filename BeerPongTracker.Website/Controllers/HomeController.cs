﻿using BeerPongTracker.ApiClient.Client;
using BeerPongTracker.Website.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace BeerPongTracker.Website.Controllers
{
    using ApiClient.ContractObjects;
    using ViewBuilders;
    public class HomeController : Controller
    {
        private readonly IBeerBongTrackerApiClient _beerBongTrackerApiClient;

        private readonly GameViewBuilder _gameViewBuilder;

        public HomeController()
        {
            _beerBongTrackerApiClient = new BeerBongTrackerApiClient(ConfigurationManager.AppSettings["ApiUrl"]);
            _gameViewBuilder = new GameViewBuilder(_beerBongTrackerApiClient);
        }

        public ActionResult Index()
        {
            var screen1ViewModel = new Screen1ViewModel();

            var availableGameData = new List<WatchGameButtonViewModel>();

            var availableGameDataFromApi = _beerBongTrackerApiClient.GetAvailableGames();

            foreach (var availableGameFromApi in availableGameDataFromApi.AvailableGameDatas)
            {
                availableGameData.Add(
                    new WatchGameButtonViewModel
                    {
                        GameId = availableGameFromApi.GameId,
                        Hint = availableGameFromApi.Hint,
                        StartDate = availableGameFromApi.StartDate
                    });
            }

            screen1ViewModel.AvailableGamesData = availableGameData;

            var model = new ScreenViewModel()
            {
                Screen1ViewModel = screen1ViewModel,
                Screen2ViewModel = new Screen2ViewModel()
            };

            return View(model);
        }

        public ActionResult PlayerNameHelper(string query, int t, int p)
        {
            var searchResults = _beerBongTrackerApiClient.PlayerSearch(query);

            if (searchResults.PlayerSearchResults.Any() == false)
            {
                return new EmptyResult();
            }

            var viewModel = new PlayerNameHelperViewModel();

            var details = new List<PlayerNameHelperPlayerDetailsViewModel>();

            foreach (var searchResult in searchResults.PlayerSearchResults)
            {
                details.Add(
                    new PlayerNameHelperPlayerDetailsViewModel
                    {
                        FacebookId = searchResult.FacebookId,
                        PlayerName = searchResult.Name,
                        PlayerId = searchResult.PlayerId
                    });
            }

            viewModel.Details = details;
            viewModel.TeamId = t;
            viewModel.PlayerId = p;

            return PartialView("_PlayerNameHelper", viewModel);
        }

        public ActionResult GetWinnerDetails(DeclareWinnerRequest request)
        {
            var viewModel = _gameViewBuilder.BuildWinnerScreenViewModel(request);

            if (string.IsNullOrEmpty(viewModel.YouTubeVideoModel.VideoId))
            {
                viewModel.YouTubeVideoModel.VideoId = ConfigurationManager.AppSettings["DefaultMusic"];
            }

            return PartialView("_4", viewModel);
        }

        public ActionResult GetEntranceScreen(int gameId, int teamId)
        {
            var details = _beerBongTrackerApiClient.GetEntranceDetails(gameId, teamId);

            var viewModel = new EntranceScreenViewModel()
            {
                FacebookId = details.FacebookId,
                Name = details.Name,
                YouTubeVideoModel = new YouTubeVideoModel()
                {
                    VideoId = details.YouTubeVideoId
                }
            };

            if (string.IsNullOrEmpty(viewModel.YouTubeVideoModel.VideoId))
            {
                viewModel.YouTubeVideoModel.VideoId = ConfigurationManager.AppSettings["DefaultMusic"];
            }

            return PartialView("_Entrance", viewModel);
        }

        public void RegisterGenericEvent(string sig, int gid)
        {
            _beerBongTrackerApiClient.RegisterGenericEvent(sig, gid);
        }

        public ActionResult DialOfDoom()
        {
            return PartialView("_DialOfDoom");
        }
    }
}
