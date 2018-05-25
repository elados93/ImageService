using System.Windows.Controls;
using ImageServiceGUI.ViewModel;
using ImageServiceGUI.Model;
using System.Threading;

namespace ImageServiceGUI.Views
{
    public partial class LogView : UserControl
    {
        LogViewModel logVm;
        public LogView()
        {
            InitializeComponent();
            logVm = new LogViewModel();
            this.DataContext = logVm;
            //logs.ItemsSource = logVm.vm_LogMessages;
        }
    }
}
