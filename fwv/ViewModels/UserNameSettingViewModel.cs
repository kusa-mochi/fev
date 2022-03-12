using System;
using System.Collections.Generic;
using System.Text;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using fwv.Common;

namespace fwv.ViewModels
{
    public class UserNameSettingViewModel : BindableBase, IDialogAware
    {
        private string _userName = "dummy";
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OkCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand(ExecuteOkCommand));
        void ExecuteOkCommand()
        {
            DialogResultParameters p = new DialogResultParameters();
            p.AddRange(new Dictionary<string, object>
            {
                { "UserName", UserName }
            });
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        #region Implementation of IDialogAware

        public string Title => "User Name Setting";

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

        #endregion
    }
}
