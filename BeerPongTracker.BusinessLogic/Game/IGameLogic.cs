namespace BeerPongTracker.BusinessLogic.Game
{
    public interface IGameLogic
    {
        StartGameResponse StartGame(StartGameRequest startGameRequest);

        Game Game(int gameId);
    }
}
