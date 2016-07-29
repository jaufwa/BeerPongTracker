namespace BeerPongTracker.ApiClient.Client
{
    using System.IO;
    using System.Net;
    using System.Text;

    using Newtonsoft.Json;

    public class BeerBongTrackerApiClient : IBeerBongTrackerApiClient
    {
        private readonly string _apiBaseUrl;

        public BeerBongTrackerApiClient(string apiBaseUrl)
        {
            _apiBaseUrl = apiBaseUrl;
        }

        public TResponse Post<TRequest, TResponse>(string url, TRequest request)
        {
            var webRequest = WebRequest.Create($"{_apiBaseUrl}/{url}") as HttpWebRequest;

            webRequest.Method = "POST";
            webRequest.ContentType = webRequest.Accept = "application/json";

            var requestJson = JsonConvert.SerializeObject(request);

            var bytes = Encoding.Default.GetBytes(requestJson);

            webRequest.GetRequestStream().Write(bytes, 0, bytes.Length);

            var response = webRequest.GetResponse() as HttpWebResponse;

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var buffer = sr.ReadToEnd();

                var responseObject = JsonConvert.DeserializeObject<TResponse>(buffer);

                return responseObject;
            }
        }

        public TResponse Get<TResponse>(string url)
        {
            var webRequest = WebRequest.Create($"{_apiBaseUrl}/{url}") as HttpWebRequest;

            webRequest.Method = "GET";
            webRequest.ContentType = webRequest.Accept = "application/json";

            var response = webRequest.GetResponse() as HttpWebResponse;

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                var buffer = sr.ReadToEnd();

                var responseObject = JsonConvert.DeserializeObject<TResponse>(buffer);

                return responseObject;
            }
        }
    }
}