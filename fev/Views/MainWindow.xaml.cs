using System.Windows;

using fev.Common;

namespace fev.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
