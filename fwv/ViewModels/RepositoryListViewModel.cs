using System;
using System.Collections.Generic;
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
        private GitManager _git = GitManager.GetInstance();
        private FileWatcher _fileWatcher = new FileWatcher();

        #endregion

        #region Properties

        private ObservableCollection<RepositoryListItem> _repositories = new ObservableCollection<RepositoryListItem>();
        public ObservableCollection<RepositoryListItem> Repositories
        {
            get { return _repositories; }
            set { SetProperty(ref _repositories, value); }
        }

        private RepositoryListItem _activeItem = null;
        public RepositoryListItem ActiveItem
        {
            get { return _activeItem; }
            set { SetProperty(ref _activeItem, value); }
        }

        #endregion

        #region Commands

        private DelegateCommand _CreateRepositoryCommand;
        public DelegateCommand CreateRepositoryCommand =>
            _CreateRepositoryCommand ?? (_CreateRepositoryCommand = new DelegateCommand(ExecuteCreateRepositoryCommand));
        void ExecuteCreateRepositoryCommand()
        {
            // TODO
            throw new NotImplementedException();
        }

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

                        _git.WorkingDirectory = workingDirectory;
                        CommandOutput gitResult = _git.Clone(repositoryUrl, workingDirectory);

                        // TODO: if cloning is done successfully.

                        RepositoryListItem newItem = new RepositoryListItem
                        {
                            IsModified = false,
                            RepositoryUrl = repositoryUrl,
                            LocalDirectoryPath = workingDirectory
                        };

                        Repositories.Add(newItem);

                        // start watching.
                        _fileWatcher.AddDirectory(newItem.Hash, newItem.LocalDirectoryPath);
                        break;
                    case ButtonResult.Cancel:
                        System.Windows.MessageBox.Show("Cancel button !!!");
                        break;
                    default:
                        break;
                }
            });
        }

        private DelegateCommand<string> _removeRepositoryCommand;
        public DelegateCommand<string> RemoveRepositoryCommand =>
            _removeRepositoryCommand ?? (_removeRepositoryCommand = new DelegateCommand<string>(ExecuteRemoveRepositoryCommand));
        void ExecuteRemoveRepositoryCommand(string parameter)
        {
            foreach (RepositoryListItem item in Repositories)
            {
                if (parameter == item.Hash)
                {
                    // TODO: remove local directories/files.

                    Repositories.Remove(item);
                    break;
                }
            }
        }

        #endregion

        #region Methods

        private void OnFilesModified(object sender, ModifiedEventArgs args)
        {
            // a hash to recognize which directory is modified.
            string watcherHash = args.WatcherHash;

            string workingDirectory = "";
            foreach (RepositoryListItem item in Repositories)
            {
                if (item.Hash == watcherHash)
                {
                    workingDirectory = item.LocalDirectoryPath;
                }
            }

            _git.EnqueueCommand(new GitAddCommandItem(workingDirectory));
            _git.EnqueueCommand(new GitCommitCommandItem(workingDirectory));
            _git.EnqueueCommand(new GitPushCommandItem(workingDirectory));
        }

        #endregion

        #region Constructors

        public RepositoryListViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            this._regionManager = regionManager;
            this._dialogService = dialogService;
            this._fileWatcher.Modified += OnFilesModified;
        }

        #endregion
    }
}