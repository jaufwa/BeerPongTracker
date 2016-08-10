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

        public PlayerSearchResponse PlayerSearch(string query)
        {
            return GetRequest<PlayerSearchResponse>($"Game/PlayerSearch?query={query}");
        }

        public Game CupSwitch(CupSwitchRequest request)
        {
            return PostRequest<CupSwitchRequest, Game>("Game/CupSwitch", request);
        }

        public StartGameResponse StartGame(StartGameRequest request)
        {
            return PostRequest<StartGameRequest, StartGameResponse>("Game/StartGame", request);
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

        private TResponse PostRequest<TRequest, TResponse>(string relativeUrl, TRequest request)
        {
            var webRequest = WebRequest.Create($"{_apiBaseUrl}/{relativeUrl}") as HttpWebRequest;

            webRequest.ContentType = webRequest.Accept = "application / json";

            webRequest.Method = "POST";

            byte[] _byteVersion = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(request));

            webRequest.ContentLength = _byteVersion.Length;

            var stream = webRequest.GetRequestStream();

            stream.Write(_byteVersion, 0, _byteVersion.Length);

            stream.Close();

            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                var responseJson = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<TResponse>(responseJson);
            }
        }
    }
}