using System.Windows.Controls;
using ImageServiceGUI.ViewModel;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.Views
{
    public partial class LogView : UserControl
    {
        LogViewModel logVm;
        public LogView()
        {
            InitializeComponent();
            logVm = new LogViewModel(new LogModel());
            this.DataContext = logVm;
        }
    }
}
