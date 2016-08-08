namespace BeerPongTracker.Website.Models
{
    public class AvatarViewModel
    {
        public string FacebookId { get; set; }
    }

    public class HealthViewModel
    {
        public int TeamId { get; set; }
        public int HealthPercentage { get; set; }
    }

    public class TeamNameViewModel
    {
        public string Name { get; set; }
    }

    public class TeamStatsViewModel
    {
        public AvatarViewModel AvatarViewModel { get; set; }

        public TeamNameViewModel TeamNameViewModel { get; set; }

        public HealthViewModel HealthViewModel { get; set; }

        public string Alignment { get; set; }

        public TeamStatsViewModel WithAlignment(string alignment)
        {
            Alignment = alignment;
            return this;
        }
    }
}