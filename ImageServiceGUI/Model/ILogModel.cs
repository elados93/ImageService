using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace ImageServiceGUI.Model
{
    interface ILogModel : INotifyPropertyChanged
    {
        ObservableCollection<Entry> LogMessages { get; set; }
    }
}
