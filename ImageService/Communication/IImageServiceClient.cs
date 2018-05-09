using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public interface IImageServiceClient
    {
        event UpdateResponseArrived UpdateAllClients;
        void sendCommand(MessageCommand commandRecievedEventArgs);
        void recieveCommand();
        void CloseClient();
    }
}
