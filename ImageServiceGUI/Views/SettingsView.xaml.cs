using ImageServiceGUI.ViewModel;
using ImageServiceGUI.Model;
using System.Windows.Controls;

namespace ImageServiceGUI.Views
{
    public partial class SettingsView : UserControl
    {
        SettingViewModel settingsVm;
        public SettingsView()
        {
            InitializeComponent();
            settingsVm = new SettingViewModel(new SettingsModel());
            this.DataContext = settingsVm;
            handlersListBox.ItemsSource = settingsVm.vm_Handlers; // The list of the handlers.
        }
    }
}
