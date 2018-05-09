using ImageService.Communication;
using ImageServiceGUI.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface IWindowModel: INotifyPropertyChanged

    {
        bool ClientConnected { get; set; }
        IImageServiceClient TcpClient { get; set; }
    }
}
