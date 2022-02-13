using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Regions;

using fev.Models;

namespace fev.ViewModels
{
    public class RepositoryListViewModel : BindableBase
    {
        public IRegionManager RegionManager { get; }

        private ObservableCollection<RepositoryListItem> _repositories = null;
        public ObservableCollection<RepositoryListItem> Repositories
        {
            get { return _repositories; }
            set { SetProperty(ref _repositories, value); }
        }

        public RepositoryListViewModel(IRegionManager regionManager)
        {
            this.RegionManager = regionManager;

            // TODO: only for debugging -->
            _repositories = new ObservableCollection<RepositoryListItem>() {
                new RepositoryListItem() {
                    RepositoryUrl = "https://aaa.bbb.com",
                    LocalDirectoryPath = @"c:\xxx\yyy\"
                },
                new RepositoryListItem() {
                    RepositoryUrl = "https://ccc.ddd.com",
                    LocalDirectoryPath = @"c:\zzz\yyy\"
                },
                new RepositoryListItem() {
                    RepositoryUrl = "https://eee.fff.com",
                    LocalDirectoryPath = @"c:\xxx\zzz\"
                },
                new RepositoryListItem() {
                    RepositoryUrl = "https://aaa.bbb.com",
                    LocalDirectoryPath = @"c:\xxx\yyy\"
                },
                new RepositoryListItem() {
                    RepositoryUrl = "https://aaa.bbb.com",
                    LocalDirectoryPath = @"c:\xxx\yyy\"
                }
            };
            // <-- only for debugging
        }
    }
}