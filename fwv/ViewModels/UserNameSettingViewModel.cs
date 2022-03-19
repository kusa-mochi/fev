using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

using fwv.Common;

namespace fwv.ViewModels
{
    public class UserNameSettingViewModel : BindableBase, IDialogAware
    {
        #region Properties

        #region UserName

        private string _userName = string.Empty;
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region EmailAddress

        private string _emailAddress = string.Empty;
        public string EmailAddress
        {
            get { return _emailAddress; }
            set
            {
                SetProperty(ref _emailAddress, value);
                OkCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #endregion

        #region Commands

        private DelegateCommand _okCommand;
        public DelegateCommand OkCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand(ExecuteOkCommand, CanExecuteOkCommand));
        void ExecuteOkCommand()
        {
            DialogResultParameters p = new DialogResultParameters();
            p.AddRange(new Dictionary<string, object>
            {
                { "UserName", UserName },
                { "EmailAddress", EmailAddress }
            });
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }
        bool CanExecuteOkCommand()
        {
            bool isValidUserName = !string.IsNullOrWhiteSpace(UserName) && !Regex.IsMatch(UserName, @"\s");
            bool isValidEmailAddress = !string.IsNullOrWhiteSpace(EmailAddress) && !Regex.IsMatch(EmailAddress, @"\s");
            return isValidUserName && isValidEmailAddress;
        }

        #endregion

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
