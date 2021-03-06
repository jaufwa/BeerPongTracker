﻿using System;
using System.Web.Http;
using BeerPongTracker.BusinessLogic.Game;
using BeerPongTracker.DataAccess.Model;

namespace BeerPongTracker.Api.Controllers
{
    using BusinessLogic.Cup;
    using Game = BeerPongTracker.BusinessLogic.Game.Game;

    [RoutePrefix("api/Game")]
    public class GameController : ApiController
    {
        private readonly IGameLogic _gameLogic;

        public GameController()
        {
            var entities = new BeerPongFederationEntities();
            _gameLogic = new GameLogic(entities);
        }

        [Route("Ping")]
        public string Ping()
        {
            return DateTime.Now.ToString();
        }

        [Route("StartGame")]
        public StartGameResponse StartGame(StartGameRequest startGameRequest)
        {
            return _gameLogic.StartGame(startGameRequest);
        }

        [HttpGet]
        [Route("Game")]
        public Game Game(int gameId)
        {
            return _gameLogic.Game(gameId);
        }

        [Route("CupSwitch")]
        public Game CupSwitch(CupSwitchRequest cupSwitchRequest)
        {
            return _gameLogic.CupSwitch(cupSwitchRequest);
        }

        [HttpGet]
        [Route("PlayerSearch")]
        public PlayerSearchResponse PlayerSearch(string query)
        {
            return _gameLogic.PlayerSearch(query);
        }

        [HttpGet]
        [Route("GetGames")]
        public GetAvailableGamesResponse GetAvailableGames()
        {
            return _gameLogic.GetAvailableGames();
        }

        [HttpGet]
        [Route("RegisterGenericEvent")]
        public void RegisterGenericEvent(string sig, int gid)
        {
            _gameLogic.RegisterGenericEvent(sig, gid);
        }

        public EntranceDetails GetEntranceDetails(string gid, string tid)
        {
            return _gameLogic.GetEntranceDetails(gid, tid);
        }

        [Route("ListenForChange")]
        public ListenForChangeResult ListenForChange(ListenForChangeRequest request)
        {
            return _gameLogic.ListenForChange(request);
        }

        [Route("DeclareWinner")]
        public DeclareWinnerResponse DeclareWinner(DeclareWinnerRequest request)
        {
            return _gameLogic.DeclareWinner(request);
        }

        [HttpPost]
        [Route("GetWinnerDetails")]
        public GetWinnerDetailsResult GetWinnerDetails(DeclareWinnerRequest request)
        {
            return _gameLogic.GetWinnerDetails(request);
        }
    }
}
