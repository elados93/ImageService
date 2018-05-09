using Communication;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    public delegate void SendCommandToServer(MessageCommand responseObj);

    public interface ISettingsModel : INotifyPropertyChanged
    {
        ObservableCollection<string> Handlers { get; set; }
        int ThumbnailsSize { get; set; }
        string LogName { get; set; }
        string SourceName { get; set; }
        string OutputDir { get; set; }
        void SendByImageService(MessageCommand msgToSend);
    }
}
