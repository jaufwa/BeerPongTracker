using BeerPongTracker.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BeerPongTracker.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new ScreenViewModel()
            {
                Screen1ViewModel= new Screen1ViewModel()
            };

            return View(model);
        }

        public ActionResult PlayerNameHelper(string query, int t, int p)
        {
            var viewModel = new PlayerNameHelperViewModel();

            var details = new List<PlayerNameHelperPlayerDetailsViewModel>();

            details.Add(new PlayerNameHelperPlayerDetailsViewModel {FacebookId = "548140192", PlayerName = "Jonny Miles", PlayerId = 1});
            details.Add(new PlayerNameHelperPlayerDetailsViewModel {FacebookId = "682905112", PlayerName = "Danny Winstone", PlayerId = 2});

            viewModel.Details = details;
            viewModel.TeamId = t;
            viewModel.PlayerId = p;

            return PartialView("_PlayerNameHelper", viewModel);
        }
    }
}
