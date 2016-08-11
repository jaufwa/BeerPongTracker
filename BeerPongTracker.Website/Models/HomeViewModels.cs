using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerPongTracker.Website.Models
{
    public class ScreenViewModel
    {
        public Screen1ViewModel Screen1ViewModel { get; set; }
        public Screen2ViewModel Screen2ViewModel { get; set; }
    }

    public class Screen1ViewModel
    {
        public IEnumerable<WatchGameButtonViewModel> AvailableGamesData { get; set; }
    }

    public class WatchGameButtonViewModel
    {
        public DateTime StartDate { get; set; }

        public string Hint { get; set; }

        public int GameId { get; set; }
    }

    public class Screen2ViewModel
    {

    }

    public class TeamInputTextViewModel
    {
        public int TeamId { get; set; }
    }

    public class TeamInputTextBoxViewModel
    {
        public int TeamId { get; set; }

        public int PlayerId { get; set; }
    }

    public class PlayerNameHelperViewModel
    {
        public IEnumerable<PlayerNameHelperPlayerDetailsViewModel> Details { get; set; }

        public int TeamId { get; set; }

        public int PlayerId { get; set; }
    }

    public class PlayerNameHelperPlayerDetailsViewModel
    {
        public string FacebookId { get; set; }

        public int PlayerId { get; set; }

        public string PlayerName { get; set; }

        public bool IsLast { get; set; }

        public void MarkIsLast()
        {
            IsLast = true;
        }
    }
}