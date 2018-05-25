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

        /// <summary>
        /// this functions gets a tcp client. it opens a stream and waits for a request of
        /// some tcp client, and it parse the request using json and sends it to the relevent
        /// class to deal with. if this is a handler it invokes the command recive delegate to see
        /// if we shuold up date all the handlers maybe about removing it from the service. the other
        /// option is activating the controller to execute some command.
        /// </summary>
        /// <param name="client"></param> is the client that sends the request.
        /// 
        void HandleClient(TcpClient client);

        // event to invoke when the request deals with a handler.
        event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        // event to invoke when a gui window is closed
        event TcpClientDelegate ExcludeClient;
    }
}
