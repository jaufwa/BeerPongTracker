using BeerPongTracker.Website.Models;

namespace BeerPongTracker.Website.ViewBuilders
{
    public class ViewBuilderResult
    {
        public ViewBuilderResult(string viewPath, GameViewModel viewModel)
        {
            ViewPath = viewPath;
            ViewModel = viewModel;
        }

        public string ViewPath { get; private set; }

        public GameViewModel ViewModel { get; private set; }
    }
}