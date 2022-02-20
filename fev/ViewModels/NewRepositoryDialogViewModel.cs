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
            set { SetProperty(ref _repositoryPlace, value); }
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
