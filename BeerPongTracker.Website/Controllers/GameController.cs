using System.Collections.Generic;
using System.Web.Mvc;
using BeerPongTracker.Website.Models;

namespace BeerPongTracker.Website.Controllers
{
    using System.Configuration;

    using BeerPongTracker.ApiClient.Client;
    using BeerPongTracker.Website.ViewBuilders;
    using Mocks;
    using ApiClient.ContractObjects;
    using Newtonsoft.Json;
    public class GameController : Controller
    {
        private readonly IBeerBongTrackerApiClient _beerBongTrackerApiClient;

        public GameController()
        {
            _beerBongTrackerApiClient = new BeerBongTrackerApiClient(ConfigurationManager.AppSettings["ApiUrl"]);
        }

        public ActionResult Build(int gameId)
        {
            var gameViewBuilder = new GameViewBuilder(_beerBongTrackerApiClient);

            var viewHelper = gameViewBuilder.Build(gameId);

            return PartialView(viewHelper);
        }

        [HttpPost]
        public JsonResult CupSwitch(CupSwitchRequest request)
        {
            return Json(_beerBongTrackerApiClient.CupSwitch(request));
        }

        public JsonResult StartGame(StartGameRequest request)
        {
            return Json(_beerBongTrackerApiClient.StartGame(request));
        }

        [HttpGet]
        public JsonResult GetGame(int gameId)
        {
            return Json(_beerBongTrackerApiClient.GetGame(gameId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListenForChange(ListenForChangeRequest request)
        {
            return Json(_beerBongTrackerApiClient.ListenForChange(request));
        }
    }
}