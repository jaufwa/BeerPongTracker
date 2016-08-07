namespace BeerPongTracker.ApiClient.Client
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using ContractObjects;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Net.Http.Headers;
    public class BeerBongTrackerApiClient : IBeerBongTrackerApiClient
    {
        private readonly string _apiBaseUrl;

        public BeerBongTrackerApiClient(string apiBaseUrl)
        {
            _apiBaseUrl = apiBaseUrl;
        }

        public Game GetGame(int gameId)
        {
            return GetRequest<Game>($"Game/Game?gameid={gameId}");
        }

        private TResponse GetRequest<TResponse>(string relativeUrl)
        {
            var webRequest = WebRequest.Create($"{_apiBaseUrl}/{relativeUrl}");

            webRequest.Method = "GET";

            var responseStream = webRequest.GetResponse().GetResponseStream();

            var respsoneJson = new StreamReader(responseStream).ReadToEnd();

            var responseObject = JsonConvert.DeserializeObject<TResponse>(respsoneJson);

            return responseObject;
        }
    }
}