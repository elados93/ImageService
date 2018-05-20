using Communication;
using ImageService.Modal.Events;
using System;

namespace ImageService.Communication
{
    interface IServer
    {
        /// <summary>
        /// start listening, and waiting for clients to be connected.
        /// </summary>
        void Start();

        /// <summary>
        /// loop throght all the clients in the list and close each client.
        /// </summary>
        void Stop();

        /// <summary>
        /// this function loop throght all the tcp clients and notify them about the 
        /// message.
        /// </summary>
        /// <param name="message"></param> the message to transfer.
        void notifyAllClients(MessageCommand message);
    }
}
