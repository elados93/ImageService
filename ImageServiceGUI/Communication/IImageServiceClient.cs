using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    public interface IImageServiceClient
    {
        event UpdateResponseArrived UpdateAllModels;
        void sendCommand(MessageCommand commandRecievedEventArgs);
        void recieveCommand();
        void CloseClient();
        bool ClientConnected { get; set; }

        void startClosingWindow();
    }
}
