using BeerPongTracker.Core.Enums;
using BeerPongTracker.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerPongTracker.BusinessLogic.Game
{
    public class GameLogic : IGameLogic
    {
        private readonly BeerPongFederationEntities _beerPongFederationEntities;

        public GameLogic(BeerPongFederationEntities beerPongFederationEntities)
        {
            _beerPongFederationEntities = beerPongFederationEntities;
        }

        public StartGameResponse StartGame(StartGameRequest startGameRequest)
        {
            // Add the game
            var game = new DataAccess.Model.Game()
            {
                DateStarted = DateTime.Now
            };

            _beerPongFederationEntities.Game.Add(game);

            _beerPongFederationEntities.SaveChanges();

            // Settings
            var gameSetting = new GameSetting()
            {
                GameId = game.GameId,
                SettingId = (int)SettingEnum.NumberOfCups,
                Value = startGameRequest.NumberOfCups.ToString()
            };

            _beerPongFederationEntities.GameSetting.Add(gameSetting);

            _beerPongFederationEntities.SaveChanges();

            // Save new players
            foreach (var team in startGameRequest.Teams)
            {
                foreach (var player in team.Players.Where(x => x.NewPlayer))
                {
                    var newPlayer = new Player();

                    newPlayer.Name = player.Name;

                    _beerPongFederationEntities.Player.Add(newPlayer);

                    _beerPongFederationEntities.SaveChanges();

                    player.PlayerId = newPlayer.PlayerId;
                }
            }

            // Set teams
            var teamId = 0;

            foreach (var team in startGameRequest.Teams)
            {
                teamId++;

                team.TeamId = teamId;

                foreach (var player in team.Players)
                {
                    var playerGame = new PlayerGame()
                    {
                        PlayerId = player.PlayerId.Value,
                        GameId = game.GameId,
                        TeamId = teamId
                    };

                    _beerPongFederationEntities.PlayerGame.Add(playerGame);

                    player.TeamId = teamId;
                }
            }

            // Set cups
            foreach (var team in startGameRequest.Teams)
            {
                for (int i = 1; i <= startGameRequest.NumberOfCups; i++)
                {
                    var cupTracker = new CupTracker()
                    {
                        GameId = game.GameId,
                        TeamId = team.TeamId.Value,
                        CupId = i,
                        Active = true
                    };

                    _beerPongFederationEntities.CupTracker.Add(cupTracker);
                }
            }

            _beerPongFederationEntities.SaveChanges();

            return new StartGameResponse();
        }
    }
}
