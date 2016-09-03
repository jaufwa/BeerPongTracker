using BeerPongTracker.BusinessLogic.Cup;

namespace BeerPongTracker.BusinessLogic.Game
{
    public interface IGameLogic
    {
        StartGameResponse StartGame(StartGameRequest startGameRequest);

        Game Game(int gameId);

        Game CupSwitch(CupSwitchRequest cupSwitchRequest);

        PlayerSearchResponse PlayerSearch(string query);

        GetAvailableGamesResponse GetAvailableGames();

        ListenForChangeResult ListenForChange(ListenForChangeRequest request);

        DeclareWinnerResponse DeclareWinner(DeclareWinnerRequest request);

        GetWinnerDetailsResult GetWinnerDetails(DeclareWinnerRequest request);

        void RegisterGenericEvent(string signature, int gameId);
    }
}
