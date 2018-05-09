using Communication;
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
    public delegate void TcpClientDelegate(TcpClient client);

    public interface IClientHandler
    {
        Mutex Mutex { get; set; }
        void HandleClient(TcpClient client);
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        event TcpClientDelegate ExcludeClient;
    }
}
