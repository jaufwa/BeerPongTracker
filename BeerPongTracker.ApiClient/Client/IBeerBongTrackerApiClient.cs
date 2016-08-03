using BeerPongTracker.ApiClient.ContractObjects;

namespace BeerPongTracker.ApiClient.Client
{
    public interface IBeerBongTrackerApiClient
    {
        Game GetGame(int gameId);
    }
}