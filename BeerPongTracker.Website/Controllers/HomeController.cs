﻿using BeerPongTracker.ApiClient.Client;
using BeerPongTracker.Website.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace BeerPongTracker.Website.Controllers
{
    using BeerPongTracker.Website.Mocks;

    public class HomeController : Controller
    {
        private readonly IBeerBongTrackerApiClient _beerBongTrackerApiClient;

        public HomeController()
        {
            //_beerBongTrackerApiClient = new BeerBongTrackerApiClient(ConfigurationManager.AppSettings["ApiUrl"]);
            _beerBongTrackerApiClient = new MockBeerBongTrackerApiClient();
        }

        public ActionResult Index()
        {
            var model = new ScreenViewModel()
            {
                Screen2ViewModel= new Screen2ViewModel()
            };

            return View(model);
        }

        public ActionResult PlayerNameHelper(string query, int t, int p)
        {
            var searchResults = _beerBongTrackerApiClient.PlayerSearch(query);
            
            var viewModel = new PlayerNameHelperViewModel();

            var details = new List<PlayerNameHelperPlayerDetailsViewModel>();

            foreach(var searchResult in searchResults.PlayerSearchResults)
            {
                details.Add(
                    new PlayerNameHelperPlayerDetailsViewModel {
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
    }
}
