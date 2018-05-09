using Communication;
using ImageService.Modal.Events;
using System;

namespace ImageService.Communication
{
    interface IServer
    {
        void Start();
        void Stop();
        void notifyAllClients(MessageCommand message);
    }
}
