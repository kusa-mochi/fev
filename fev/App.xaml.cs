﻿using fev.Views;
using Prism.Ioc;
using System.Windows;

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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Checkout>();
            containerRegistry.RegisterForNavigation<Explorer>();
        }
    }
}