using System.Windows;
using MaterialDesignExtensions.Controls;

using fwv.Common;

namespace fwv.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MaterialWindow
    {
        public MainWindow()
        {
            _logManager.AppendLog("begin.");
            InitializeComponent();
            _logManager.AppendLog("fin.");
        }

        private LogManager _logManager = LogManager.GetInstance();
    }
}
