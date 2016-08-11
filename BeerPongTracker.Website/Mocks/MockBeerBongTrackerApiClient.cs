using BeerPongTracker.ApiClient.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeerPongTracker.ApiClient.ContractObjects;

namespace BeerPongTracker.Website.Mocks
{
    public class MockBeerBongTrackerApiClient : IBeerBongTrackerApiClient
    {
        public Game GetGame(int gameId)
        {
            var gameMap = new Dictionary<int, Game>()
            {
                {1, GetGame1()},
                {2, GetGame2()},
                {3, GetGame3()}
            };

            return gameMap[gameId];
        }

        public Game GetGame1()
        {
            var game = new Game() {
                GameId = 1,
                NumberOfCups = 10,
                NumberOfTeams = 2,
                Teams = new List<Team>()
            };

            game.Teams.Add(BuildTeam1());
            game.Teams.Add(BuildTeam2());

            return game;
        }

        public Game GetGame2()
        {
            var game = new Game()
            {
                GameId = 2,
                NumberOfCups = 10,
                NumberOfTeams = 3,
                Teams = new List<Team>()
            };

            game.Teams.Add(BuildTeam1());
            game.Teams.Add(BuildTeam2());
            game.Teams.Add(BuildTeam3());

            return game;
        }

        public Game GetGame3()
        {
            var game = new Game()
            {
                GameId = 1,
                NumberOfCups = 15,
                NumberOfTeams = 2,
                Teams = new List<Team>()
            };

            game.Teams.Add(BuildTeam4());
            game.Teams.Add(BuildTeam5());

            return game;
        }

        private Team BuildTeam1()
        {
            var result = GetTeam1();

            result.CupStats = Get10Cups1();

            return result;
        }

        private Team BuildTeam2()
        {
            var result = GetTeam2();

            result.CupStats = Get10Cups2();

            return result;
        }

        private Team BuildTeam3()
        {
            var result = GetTeam3();

            result.CupStats = Get10Cups1();

            return result;
        }

        private Team BuildTeam4()
        {
            var result = GetTeam1();

            result.CupStats = Get15Cups1();

            return result;
        }

        private Team BuildTeam5()
        {
            var result = GetTeam2();

            result.CupStats = Get15Cups2();

            return result;
        }

        private Team GetTeam1()
        {
            return new Team()
            {
                FacebookId = "548140192",
                Health = 20,
                TeamId = 1,
                TeamName = "Jonny Miles"
            };
        }

        private Team GetTeam2()
        {
            return new Team()
            {
                FacebookId = "666875244",
                Health = 80,
                TeamId = 2,
                TeamName = "Chris Tucker"
            };
        }

        private Team GetTeam3()
        {
            return new Team()
            {
                FacebookId = "682905112",
                Health = 50,
                TeamId = 3,
                TeamName = "Danny Winstone"
            };
        }

        private List<CupStats> Get10Cups1()
        {
            return new List<CupStats>()
            {
                {new CupStats() {TeamId = 1, Active = false, CupId = 1 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 2 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 3 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 4 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 5 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 6 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 7 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 8 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 9 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 10 }}
            };
        }

        private List<CupStats> Get10Cups2()
        {
            return new List<CupStats>()
            {
                {new CupStats() {TeamId = 1, Active = true, CupId = 1 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 2 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 3 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 4 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 5 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 6 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 7 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 8 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 9 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 10 }}
            };
        }

        private List<CupStats> Get15Cups1()
        {
            return new List<CupStats>()
            {
                {new CupStats() {TeamId = 1, Active = false, CupId = 1 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 2 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 3 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 4 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 5 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 6 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 7 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 8 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 9 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 10 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 11 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 12 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 13 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 14 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 15 }}
            };
        }


        private List<CupStats> Get15Cups2()
        {
            return new List<CupStats>()
            {
                {new CupStats() {TeamId = 1, Active = false, CupId = 1 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 2 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 3 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 4 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 5 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 6 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 7 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 8 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 9 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 10 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 11 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 12 }},
                {new CupStats() {TeamId = 1, Active = false, CupId = 13 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 14 }},
                {new CupStats() {TeamId = 1, Active = true, CupId = 15 }}
            };
        }

        public Game CupSwitch(CupSwitchRequest request)
        {
            return GetGame1();
        }

        public StartGameResponse StartGame(StartGameRequest request)
        {
            return new StartGameResponse {GameId = 1};
        }

        public PlayerSearchResponse PlayerSearch(string query)
        {
            var searchResults = new List<PlayerSearchResult>();

            searchResults.Add(new PlayerSearchResult {FacebookId = "548140192", Name = "Jonny Miles", PlayerId = 1});
            searchResults.Add(new PlayerSearchResult {FacebookId = "682905112", Name = "Danny Winstone", PlayerId = 2});
            searchResults.Add(new PlayerSearchResult {FacebookId = "666875244", Name = "Chris Tucker", PlayerId = 3});
            searchResults.Add(new PlayerSearchResult {FacebookId = "839635028", Name = "Karl Winestone", PlayerId = 4});

            return new PlayerSearchResponse {PlayerSearchResults = searchResults};
        }

        public AvailableGames GetAvailableGames()
        {
            var availableGameDatas = new List<AvailableGameData>();

            availableGameDatas.Add(new AvailableGameData { GameId = 1, Hint = "JM vs CT", StartDate = DateTime.Now.AddMinutes(-30) });
            availableGameDatas.Add(new AvailableGameData { GameId = 2, Hint = "JM & CT vs DW & KW", StartDate = DateTime.Now.AddMinutes(-42) });
            availableGameDatas.Add(new AvailableGameData { GameId = 3, Hint = "JM vs CQ", StartDate = DateTime.Now.AddMinutes(-42) });

            return new AvailableGames {AvailableGameDatas = availableGameDatas};
        }

        public ListenForChangeResult ListenForChange(ListenForChangeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}