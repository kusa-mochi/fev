using System;
using System.Collections.Generic;
using System.Text;

using Prism.Mvvm;

namespace fwv.ViewModels
{
    public class UserNameSettingViewModel : BindableBase
    {
        private string _userName = "dummy";
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
    }
}
