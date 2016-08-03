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

        private Team BuildTeam1()
        {
            return new Team()
            {
                FacebookId = "548140192",
                Health = 90,
                TeamId = 1,
                TeamName = "Jonny Miles",
                CupStats = new List<CupStats>()
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
                }
            };
        }

        private Team BuildTeam2()
        {
            return new Team()
            {
                FacebookId = "666875244",
                Health = 80,
                TeamId = 2,
                TeamName = "Chris Tucker",
                CupStats = new List<CupStats>()
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
                }
            };
        }
    }
}