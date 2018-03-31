using System;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved; // Event that will notify all the functions message has been recived.
        void Log(string message, MessageTypeEnum type);           // Logging the Message
    }
}

