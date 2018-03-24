using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        void Log(string message, MessageTypeEnum type);           // Logging the Message
    }
}

