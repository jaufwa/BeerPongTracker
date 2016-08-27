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

        private readonly GameViewBuilder _gameViewBuilder;

        public GameController()
        {
            _beerBongTrackerApiClient = new BeerBongTrackerApiClient(ConfigurationManager.AppSettings["ApiUrl"]);
            _gameViewBuilder = new GameViewBuilder(_beerBongTrackerApiClient);
        }

        public ActionResult Build(BuildGameRequest request)
        {
            var viewHelper = _gameViewBuilder.Build(request.GameId, request.Controlling, request.IsPc);

            return PartialView(viewHelper);
        }

        [HttpPost]
        public JsonResult CupSwitch(CupSwitchRequest request)
        {
            return Json(_beerBongTrackerApiClient.CupSwitch(request));
        }

        public JsonResult DeclareWinner(DeclareWinnerRequest request)
        {
            return Json(_beerBongTrackerApiClient.DeclareWinner(request));
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