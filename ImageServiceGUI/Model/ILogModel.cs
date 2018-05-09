using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using ImageService.Infrastructure;

namespace ImageServiceGUI.Model
{
    interface ILogModel : INotifyPropertyChanged
    {
        ObservableCollection<Entry> LogMessages { get; set; }
    }
}
