namespace BeerPongTracker.ApiClient.ContractObjects
{
    public class CupSwitchRequest
    {
        public int GameId { get; set; }

        public int TeamId { get; set; }

        public int CupId { get; set; }
    }
}
