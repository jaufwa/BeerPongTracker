namespace BeerPongTracker.ApiClient.ContractObjects
{
    public class StartGamePlayer
    {
        public int? PlayerId { get; set; }

        public bool NewPlayer => PlayerId.HasValue == false;

        public string Name { get; set; }

        public int? TeamId { get; set; }
    }
}
