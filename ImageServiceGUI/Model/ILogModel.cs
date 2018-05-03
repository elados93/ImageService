using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace ImageServiceGUI.Model
{
    interface ILogModel : INotifyPropertyChanged
    {
        ObservableCollection<EventLogEntry> LogMessages { get; set; }
    }
}
