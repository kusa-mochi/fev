using System;
using System.Collections.Generic;
using System.Text;

using Prism.Mvvm;
using Prism.Services.Dialogs;

using fev.Common;

namespace fev.ViewModels
{
    public class NewRepositoryDialogViewModel : BindableBase, IDialogAware
    {
        #region Properties

        private RepositoryPlace _repositoryPlace;
        public RepositoryPlace RepositoryPlace
        {
            get { return _repositoryPlace; }
            set
            {
                switch (value)
                {
                    case RepositoryPlace.Remote:
                        IsRemoteEnabled = true;
                        IsLocalEnabled = false;
                        break;
                    case RepositoryPlace.Local:
                        IsRemoteEnabled = false;
                        IsLocalEnabled = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
                SetProperty(ref _repositoryPlace, value);
            }
        }

        private bool _isRemoteEnabled;
        public bool IsRemoteEnabled
        {
            get { return _isRemoteEnabled; }
            set { SetProperty(ref _isRemoteEnabled, value); }
        }

        private bool _isLocalEnabled;
        public bool IsLocalEnabled
        {
            get { return _isLocalEnabled; }
            set { SetProperty(ref _isLocalEnabled, value); }
        }

        private string _workingDirectoryPath;
        public string WorkingDirectoryPath
        {
            get { return _workingDirectoryPath; }
            set { SetProperty(ref _workingDirectoryPath, value); }
        }

        #endregion

        public string Title => "New Folder Settings";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
