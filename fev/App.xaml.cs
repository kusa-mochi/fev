using System.Windows;
using Prism.Ioc;
using fev.Views;
using fev.ViewModels;

namespace fev
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry reg)
        {
            reg.RegisterForNavigation<Checkout>();
            reg.RegisterForNavigation<RepositoryList>();
            reg.RegisterDialog<NewRepositoryDialog, NewRepositoryDialogViewModel>();
        }
    }
}
