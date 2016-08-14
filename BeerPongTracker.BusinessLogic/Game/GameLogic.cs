using System;
using System.Linq;
using BeerPongTracker.Core.Enums;
using BeerPongTracker.DataAccess.Model;
using System.Collections.Generic;
using BeerPongTracker.BusinessLogic.Cup;
using BeerPongTracker.Core;
using System.Data.SqlClient;
using System.Threading;

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
                DateStarted = DateTime.Now,
                LastUpdated = DateTime.Now,
                LastUpdateSignature = Guid.NewGuid().ToString()
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
            result.LastUpdateSignature = dbGame.LastUpdateSignature;
            result.NumberOfCups = int.Parse(dbCupsNumberSetting.Value);

            var dbPlayerGames = _beerPongFederationEntities.PlayerGame.Include("Player").Where(x => x.GameId == gameId);
            if (dbPlayerGames == null) return null;

            result.Teams = new List<Team>();

            result.NumberOfTeams = dbPlayerGames.Select(x => x.TeamId).Distinct().Count();

            foreach (var teamId in dbPlayerGames.Select(x => x.TeamId).Distinct().ToList())
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

        public DeclareWinnerResponse DeclareWinner(DeclareWinnerRequest request)
        {
            var dbGame = _beerPongFederationEntities.Game.Single(x => x.GameId == request.GameId);

            dbGame.LastUpdateSignature = $"tw-{request.WinningTeamId}-{Guid.NewGuid().ToString().Substring(0, 7)}";

            _beerPongFederationEntities.SaveChanges();

            return new DeclareWinnerResponse();
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

            var game = _beerPongFederationEntities.Game.Single(x => x.GameId == cupSwitchRequest.GameId);
            game.LastUpdated = DateTime.Now;
            game.LastUpdateSignature = $"cs-{Guid.NewGuid().ToString().Substring(0,7)}";

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

            if (sqlQuery.StartsWith("%") == false)
            {
                sqlQuery = "%" + sqlQuery;
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
                .SqlQuery<AvailableGameResult>("GetAvailableGames @StartDate", parameter)
                .ToList();

            var availableGameDatas = new List<AvailableGameData>();

            foreach (var gameId in dbResults.Select(x => x.GameId).Distinct())
            {
                var availableGameData = new AvailableGameData { GameId = gameId };
                availableGameData.StartDate = dbResults.First(x => x.GameId == gameId).DateStarted;
                availableGameDatas.Add(availableGameData);
            }

            foreach (var availableGameData in availableGameDatas)
            {
                var teamIds = dbResults.Where(x => x.GameId == availableGameData.GameId).Select(x => x.TeamId).Distinct();
                var teamNames = new List<string>();

                foreach (var teamId in teamIds)
                {
                    var playerNames = dbResults.Where(x => x.GameId == availableGameData.GameId && x.TeamId == teamId).Select(x => x.Name);
                    var teamName = StringHelper.CombinePlayerNames(playerNames);
                    teamNames.Add(teamName);
                }

                var gameHintText = string.Join(" vs ", teamNames);

                availableGameData.Hint = gameHintText;
            }

            return new GetAvailableGamesResponse {AvailableGames = availableGameDatas};
        }

        public ListenForChangeResult ListenForChange(ListenForChangeRequest request)
        {
            var periodEnd = DateTime.Now.AddSeconds(30);

            while (DateTime.Now < periodEnd)
            {
                var parameter = new SqlParameter("@GameId", request.GameId);

                var dbResults = _beerPongFederationEntities.Database
                    .SqlQuery<GetLastUpdateSignatureResponse>("GetLastUpdateSignature @GameId", parameter)
                    .ToList();

                var dbLastUpdateSignature = dbResults.First().LastUpdateSignature;

                if (request.LastUpdateSignature != dbLastUpdateSignature)
                {
                    return new ListenForChangeResult() { LastUpdateSignature = dbLastUpdateSignature, Updated = true };
                }

                Thread.Sleep(1000);
            }

            return new ListenForChangeResult() { LastUpdateSignature = request.LastUpdateSignature, Updated = false };
        }

        public GetWinnerDetailsResult GetWinnerDetails(DeclareWinnerRequest request)
        {
            var gameIdParam = new SqlParameter("@GameId", request.GameId);
            var teamIdParam = new SqlParameter("@TeamId", request.WinningTeamId);

            var dbResults = _beerPongFederationEntities.Database
                .SqlQuery<WinnerDetail>("GetWinners @GameId, @TeamId", gameIdParam, teamIdParam)
                .ToList();

            var youTubeVideoId = (dbResults.FirstOrDefault(x => x.YouTubeVideoId != null))?.YouTubeVideoId;

            return new GetWinnerDetailsResult() {
                WinnerDetails = dbResults,
                YouTubeVideoId = youTubeVideoId
            };
        }
    }
}
