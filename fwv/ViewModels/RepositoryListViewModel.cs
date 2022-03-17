using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.WindowsAPICodePack.Dialogs;
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
        private LogManager _log = LogManager.GetInstance();

        #endregion

        #region Properties

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                TopMessage = $"Hi, {_userName}";
            }
        }

        private string _topMessage;
        public string TopMessage
        {
            get { return _topMessage; }
            set { SetProperty(ref _topMessage, value); }
        }

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

        #region ValidateUserName

        private DelegateCommand _validateUserName;
        public DelegateCommand ValidateUserName =>
            _validateUserName ?? (_validateUserName = new DelegateCommand(ExecuteValidateUserName));
        void ExecuteValidateUserName()
        {
            _git.WorkingDirectory = string.Empty;
            CommandOutput commandOutput = _git.GetUserName();
            string currentUserName = commandOutput.StandardOutput;

            // if a user name is not set to git global setting,
            if (string.IsNullOrEmpty(currentUserName))
            {
                _log.AppendLog("user name is not registered yet.");

                // show a dialog for setting user name.
                _dialogService.ShowDialog(typeof(fwv.Views.UserNameSetting).Name, result =>
                {
                    IDialogParameters p = result.Parameters;

                    switch (result.Result)
                    {
                        case ButtonResult.OK:
                            {
                                string userInput = result.Parameters.GetValue<string>("UserName");
                                _git.SetUserName(userInput);
                                UserName = userInput;
                                break;
                            }
                        default:
                            {
                                fwv.App.Current.Shutdown(1);
                                break;
                            }
                    }
                });
            }
            else
            {
                UserName = currentUserName;
            }
        }

        #endregion

        #region CreateRepositoryCommand

        private DelegateCommand _CreateRepositoryCommand;
        public DelegateCommand CreateRepositoryCommand =>
            _CreateRepositoryCommand ?? (_CreateRepositoryCommand = new DelegateCommand(ExecuteCreateRepositoryCommand));
        void ExecuteCreateRepositoryCommand()
        {
            using (CommonOpenFileDialog dlg = new CommonOpenFileDialog()
            {
                Title = "Choose a directory",
                IsFolderPicker = true,
                RestoreDirectory = true,
                Multiselect = false
            })
            {
                CommonFileDialogResult result = dlg.ShowDialog();
                if (result != CommonFileDialogResult.Ok) return;

                string dirPath = dlg.FileName;
                _git.WorkingDirectory = dirPath;
                _git.Init(true);
            }
        }

        #endregion

        #region OpenNewRepositoryDialogCommand

        private DelegateCommand _openNewRepositoryDialogCommand;
        public DelegateCommand OpenNewRepositoryDialogCommand =>
            _openNewRepositoryDialogCommand ?? (_openNewRepositoryDialogCommand = new DelegateCommand(ExecuteOpenNewRepositoryDialogCommand));
        void ExecuteOpenNewRepositoryDialogCommand()
        {
            _dialogService.ShowDialog(typeof(fwv.Views.NewRepositoryDialog).Name, result =>
            {
                IDialogParameters p = result.Parameters;

                switch (result.Result)
                {
                    case ButtonResult.OK:
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
                        break;
                    default:
                        break;
                }
            });
        }

        #endregion

        #region RemoveRepositoryCommand

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

        #region OpenHistoryDialogCommand

        private DelegateCommand _OpenHistoryDialogCommand;
        public DelegateCommand OpenHistoryDialogCommand =>
            _OpenHistoryDialogCommand ?? (_OpenHistoryDialogCommand = new DelegateCommand(ExecuteOpenHistoryDialogCommand));
        void ExecuteOpenHistoryDialogCommand()
        {
            _dialogService.ShowDialog(typeof(fwv.Views.HistoryDialog).Name, result =>
            {
            });
        }

        #endregion

        #region AddRepositoryCommand

        private DelegateCommand _addRepositoryCommand;
        public DelegateCommand AddRepositoryCommand =>
            _addRepositoryCommand ?? (_addRepositoryCommand = new DelegateCommand(ExecuteAddRepositoryCommand));
        void ExecuteAddRepositoryCommand()
        {
            using (CommonOpenFileDialog dlg = new CommonOpenFileDialog()
            {
                Title = "Choose a directory",
                IsFolderPicker = true,
                RestoreDirectory = true,
                Multiselect = false
            })
            {
                CommonFileDialogResult result = dlg.ShowDialog();
                if (result != CommonFileDialogResult.Ok) return;

                string dirPath = dlg.FileName;
                dirPath = TrimEndRepositoryUrl(dirPath);

                _git.WorkingDirectory = dirPath;
                CommandOutput commandOutput = _git.GetRemoteUrl();
                if (!string.IsNullOrWhiteSpace(commandOutput.StandardError))
                {
                    _log.AppendErrorLog(commandOutput.StandardError);
                    System.Windows.MessageBox.Show("Error: This is not a Git repository.");
                    return;
                }
                string remoteUrl = commandOutput.StandardOutput;

                RepositoryListItem newItem = new RepositoryListItem
                {
                    IsModified = false,
                    RepositoryUrl = remoteUrl,
                    LocalDirectoryPath = dirPath
                };

                Repositories.Add(newItem);

                // start watching.
                _fileWatcher.AddDirectory(newItem.Hash, newItem.LocalDirectoryPath);
            }
        }

        #endregion

        #endregion

        #region Private Methods

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

        private string TrimEndRepositoryUrl(string url)
        {
            string output = string.Empty;
            string urlEnd = url.Substring(url.Length - 4);
            if (urlEnd == ".git")
            {
                output = url.Substring(0, url.Length - 4);
            }

            return output;
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