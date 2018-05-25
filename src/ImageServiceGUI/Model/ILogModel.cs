using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Infrastructure;

namespace ImageServiceGUI.Model
{
    /// <summary>
    /// Interface for log model, handels the logging.
    /// </summary>
    interface ILogModel : INotifyPropertyChanged
    {
        ObservableCollection<Entry> LogMessages { get; set; }
    }
}
