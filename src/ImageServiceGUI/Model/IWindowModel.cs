using ImageServiceGUI.Communication;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    /// <summary>
    /// Interface for window model.
    /// </summary>
    interface IWindowModel: INotifyPropertyChanged
    {
        bool ClientConnected { get; set; }
        IImageServiceClient TcpClient { get; set; }
    }
}
