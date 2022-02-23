using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using fwv.Common;
using fwv.Models;

namespace fwv.ViewModels
{
    public class RepositoryListViewModel : BindableBase
    {
        #region Fields

        private IRegionManager _regionManager = null;
        private IDialogService _dialogService = null;
        private ObservableCollection<RepositoryListItem> _repositories = new ObservableCollection<RepositoryListItem>();
        private GitManager _git = GitManager.GetInstance();

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
            _dialogService.ShowDialog(typeof(fwv.Views.NewRepositoryDialog).Name, result =>
            {
                switch (result.Result)
                {
                    case ButtonResult.OK:
                        IDialogParameters p = result.Parameters;
                        Enum.TryParse(p.GetValue<string>("RepositoryPlace"), out RepositoryPlace repositoryPlace);
                        string repositoryUrl = repositoryPlace switch
                        {
                            RepositoryPlace.Remote => p.GetValue<string>("RemoteRepositoryUrl"),
                            RepositoryPlace.Local => p.GetValue<string>("LocalBareRepositoryPath"),
                            _ => throw new Exception("invalid result param \"RepositoryPlace\"")
                        };
                        string workingDirectory = p.GetValue<string>("WorkingDirectoryPath");

                        // TODO: check if the destination directory is empty.

                        _git.Clone(repositoryUrl, workingDirectory);

                        // TODO: if cloning is done successfully.

                        Repositories.Add(
                            new RepositoryListItem
                            {
                                RepositoryUrl = repositoryUrl,
                                LocalDirectoryPath = workingDirectory
                            }
                        );
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
        }

        #endregion
    }
}