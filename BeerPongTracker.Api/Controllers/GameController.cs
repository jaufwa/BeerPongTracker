using System;
using System.Web.Http;
using BeerPongTracker.BusinessLogic.Game;
using BeerPongTracker.DataAccess.Model;

namespace BeerPongTracker.Api.Controllers
{
    using Game = BeerPongTracker.BusinessLogic.Game.Game;

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

        [Route("Game")]
        public Game Game(int gameId)
        {
            return _gameLogic.Game(gameId);
        }
    }
}
