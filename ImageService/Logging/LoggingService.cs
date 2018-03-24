using System;

namespace ImageService
{
    public class LoggingService : ILoggingService
    {
        public LoggingService()
        {

        }

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved.Invoke(this, new MessageRecievedEventArgs(type, message));
        }
    }
}
