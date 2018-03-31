using System;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public LoggingService() { }

        /// <summary>
        /// Log is used to write messages to the logs of the program.
        /// </summary>
        /// <param name="message">The wanted message.</param>
        /// <param name="type">The type of the message.</param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(type, message));
        }
    }
}
