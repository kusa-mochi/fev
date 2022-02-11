using Prism.Mvvm;
using Prism.Regions;

using fev.Models;

namespace fev.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IRegionManager RegionManager { get; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;

            // initialize content region.
            this.RegionManager.RegisterViewWithRegion("ContentRegion", typeof(fev.Views.Checkout));
        }

        public void NavigateTo(string viewName)
        {
            this.RegionManager.RequestNavigate("ContentRegion", viewName);
        }

        private GitManager _git = new GitManager();
    }
}
