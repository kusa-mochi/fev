using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using fev.Models;

namespace fev.ViewModels
{
    public class RepositoryListViewModel : BindableBase
    {
        #region Fields

        public IRegionManager _regionManager = null;
        public IDialogService _dialogService = null;
        private ObservableCollection<RepositoryListItem> _repositories = null;

        #endregion

        #region Properties

        public ObservableCollection<RepositoryListItem> Repositories
        {
            get { return _repositories; }
            set { SetProperty(ref _repositories, value); }
        }

        #endregion

        #region Commands

        private DelegateCommand _openNewRepositoryDialogCommand;
        public DelegateCommand OpenNewRepositoryDialogCommand =>
            _openNewRepositoryDialogCommand ?? (_openNewRepositoryDialogCommand = new DelegateCommand(ExecuteOpenNewRepositoryDialogCommand));

        void ExecuteOpenNewRepositoryDialogCommand()
        {
            _dialogService.ShowDialog(typeof(fev.Views.NewRepositoryDialog).Name, result =>
            {
                switch (result.Result)
                {
                    case ButtonResult.OK:
                        System.Windows.MessageBox.Show("OK button !!!");
                        break;
                    case ButtonResult.Cancel:
                        System.Windows.MessageBox.Show("Cancel button !!!");
                        break;
                    default:
                        break;
                }
            });
        }

        #endregion

        #region Constructors

        public RepositoryListViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            this._regionManager = regionManager;
            this._dialogService = dialogService;

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

        #endregion
    }
}