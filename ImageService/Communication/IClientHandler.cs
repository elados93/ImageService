using ImageService.Modal.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    interface IClientHandler
    {
        Mutex Mutex { get; set; }
        void HandleClient(TcpClient client);
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;
    }
}
