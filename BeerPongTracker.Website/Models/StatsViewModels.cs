namespace BeerPongTracker.Website.Models
{
    public class AvatarViewModel
    {
        public string FacebookId { get; set; }
    }

    public class TeamNameViewModel
    {
        public string Name { get; set; }
    }

    public class TeamStatsViewModel
    {
        public AvatarViewModel AvatarViewModel { get; set; }

        public TeamNameViewModel TeamNameViewModel { get; set; }

        public string Alignment { get; set; }
    }
}