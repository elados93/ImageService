using System;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public LoggingService() { }

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(type, message));
        }
    }
}
