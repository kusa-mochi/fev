using System;
using System.Collections.Generic;
using System.Text;

using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace fev.ViewModels
{
    public class NewRepositoryDialogViewModel : BindableBase, IDialogAware
    {
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
