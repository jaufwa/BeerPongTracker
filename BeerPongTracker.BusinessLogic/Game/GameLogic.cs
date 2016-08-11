using System;
using System.Linq;
using BeerPongTracker.Core.Enums;
using BeerPongTracker.DataAccess.Model;
using System.Collections.Generic;
using BeerPongTracker.BusinessLogic.Cup;
using BeerPongTracker.Core;
using System.Data.SqlClient;

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

            return new StartGameResponse() { GameId = game.GameId };
        }

        public Game Game(int gameId)
        {
            var result = new Game();

            var dbGame = _beerPongFederationEntities.Game.FirstOrDefault(x => x.GameId == gameId);
            if (dbGame == null) return null;

            var dbPlayers = _beerPongFederationEntities.PlayerGame.Where(x => x.GameId == gameId);
            if (dbPlayers == null) return null;

            var dbCupsNumberSetting = _beerPongFederationEntities.GameSetting.FirstOrDefault(
                x => x.GameId == gameId && x.SettingId == (int)SettingEnum.NumberOfCups);

            result.GameId = dbGame.GameId;
            result.NumberOfCups = int.Parse(dbCupsNumberSetting.Value);

            var dbPlayerGames = _beerPongFederationEntities.PlayerGame.Include("Player").Where(x => x.GameId == gameId);
            if (dbPlayerGames == null) return null;

            result.Teams = new List<Team>();

            result.NumberOfTeams = dbPlayerGames.Select(x => x.TeamId).Distinct().Count();

            foreach (var teamId in dbPlayerGames.Select(x => x.TeamId).Distinct())
            {
                var team = new Team();

                team.TeamId = teamId;

                if (dbPlayerGames.Where(x => x.TeamId == teamId).Count() == 1)
                {
                    var player = dbPlayerGames.First(x => x.TeamId == teamId).Player;
                    team.TeamName = player.Name;
                    team.FacebookId = player.FacebookId;
                }
                else
                {
                    var players = dbPlayerGames.Where(x => x.TeamId == teamId).Select(x => x.Player);
                    team.TeamName = GetCombinedTeamName(players.Select(x => x.Name));
                }

                var dbCups = _beerPongFederationEntities.CupTracker.Where(x => x.GameId == gameId && x.TeamId == teamId);

                var cupStats = new List<CupStats>();

                foreach (var dbCup in dbCups)
                {
                    var cupStat = new CupStats() {
                        Active = dbCup.Active,
                        CupId = dbCup.CupId,
                        TeamId = teamId
                    };

                    cupStats.Add(cupStat);
                }

                team.CupStats = cupStats;

                var activeCupsCount = cupStats.Where(x => x.Active).Count();

                var totalCupsCount = cupStats.Count();

                team.Health = BeerPongMath.GetPercentage(activeCupsCount, totalCupsCount);

                result.Teams.Add(team);
            }

            return result;
        }

        public Game CupSwitch(CupSwitchRequest cupSwitchRequest)
        {
            var cupTracker = _beerPongFederationEntities.CupTracker.FirstOrDefault(x =>
                x.GameId == cupSwitchRequest.GameId &&
                x.TeamId == cupSwitchRequest.TeamId &&
                x.CupId == cupSwitchRequest.CupId);

            if (cupTracker == null)
            {
                return null;
            }

            cupTracker.Active = !cupTracker.Active;

            _beerPongFederationEntities.SaveChanges();

            return Game(cupSwitchRequest.GameId);
        }

        private string GetCombinedTeamName(IEnumerable<string> playerNames)
        {
            var shortPlayerNames = Array.ConvertAll(playerNames.ToArray(), new Converter<string, string>(StringHelper.ShortenName));

            return string.Join(" + ", shortPlayerNames);
        }

        public PlayerSearchResponse PlayerSearch(string query)
        {
            var sqlQuery = query.Replace(" ", "%");

            if (sqlQuery.EndsWith("%") == false)
            {
                sqlQuery += "%";
            }

            var parameter = new SqlParameter("@Query", sqlQuery);

            var dbResults = _beerPongFederationEntities.Database
                .SqlQuery<PlayerSearchResult>("PlayerNameSearch @Query", parameter)
                .ToList();

            return new PlayerSearchResponse() { PlayerSearchResults = dbResults };
        }

        public GetAvailableGamesResponse GetAvailableGames()
        {
            var parameter = new SqlParameter("@StartDate", DateTime.Now.AddHours(-48));

            var dbResults = _beerPongFederationEntities.Database
                .SqlQuery<AvailableGameResult>("GetAvailableGames @Query", parameter)
                .ToList();

            return new GetAvailableGamesResponse();
        }
    }
}
