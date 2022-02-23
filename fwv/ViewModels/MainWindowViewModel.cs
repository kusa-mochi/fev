using Prism.Mvvm;
using Prism.Regions;

using fwv.Models;

namespace fwv.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public IRegionManager RegionManager { get; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;

            // initialize content region.
            this.RegionManager.RegisterViewWithRegion("ContentRegion", typeof(fwv.Views.RepositoryList));
        }

        public void NavigateTo(string viewName)
        {
            this.RegionManager.RequestNavigate("ContentRegion", viewName);
        }
    }
}
