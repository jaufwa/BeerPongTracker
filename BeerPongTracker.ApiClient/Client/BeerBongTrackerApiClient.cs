namespace BeerPongTracker.ApiClient.Client
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using ContractObjects;
    using Newtonsoft.Json;

    public class BeerBongTrackerApiClient : IBeerBongTrackerApiClient
    {
        private readonly string _apiBaseUrl;

        public BeerBongTrackerApiClient(string apiBaseUrl)
        {
            _apiBaseUrl = apiBaseUrl;
        }

        public Game GetGame(int gameId)
        {
            throw new NotImplementedException();
        }
    }
}