namespace BeerPongTracker.ApiClient.Client
{
    public interface IBeerBongTrackerApiClient
    {
        TResponse Post<TRequest, TResponse>(string url, TRequest request);

        TResponse Get<TResponse>(string url);
    }
}