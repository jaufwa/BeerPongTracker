namespace BeerPongTracker.Website.ViewBuilders
{
    public class ViewBuilderResult
    {
        public ViewBuilderResult(string viewPath, object viewModel)
        {
            ViewPath = viewPath;
            ViewModel = viewModel;
        }

        public string ViewPath { get; private set; }

        public object ViewModel { get; private set; }
    }
}