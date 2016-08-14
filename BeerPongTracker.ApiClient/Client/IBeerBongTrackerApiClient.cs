using BeerPongTracker.ApiClient.ContractObjects;

namespace BeerPongTracker.ApiClient.Client
{
    public interface IBeerBongTrackerApiClient
    {
        Game GetGame(int gameId);

        Game CupSwitch(CupSwitchRequest request);

        DeclareWinnerResponse DeclareWinner(DeclareWinnerRequest request);

        StartGameResponse StartGame(StartGameRequest request);

        PlayerSearchResponse PlayerSearch(string query);

        AvailableGames GetAvailableGames();

        ListenForChangeResult ListenForChange(ListenForChangeRequest request);

        GetWinnerDetailsResult GetWinnerDetails(DeclareWinnerRequest request);
    }
}