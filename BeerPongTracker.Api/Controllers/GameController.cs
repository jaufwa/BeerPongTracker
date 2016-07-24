using BeerPongTracker.BusinessLogic.Game;
using BeerPongTracker.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BeerPongTracker.Api.Controllers
{
    [RoutePrefix("api/Game")]
    public class GameController : ApiController
    {
        private readonly IGameLogic _gameLogic;

        public GameController()
        {
            _gameLogic = new GameLogic(new BeerPongFederationEntities());
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
    }
}
