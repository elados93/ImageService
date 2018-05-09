using Infrastracture.Enums;
using System;

namespace ImageService.Logging.Modal
{
    public class MessageRecievedEventArgs:EventArgs
    {
        #region Propreties
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
        #endregion

        /// <summary>
        /// Create a new MessageRecievedEventArgs object.
        /// </summary>
        /// <param name="stat">The type of the message.</param>
        /// <param name="str">The message, a string object.</param>
        public MessageRecievedEventArgs(MessageTypeEnum stat, String str)
        {
            Status = stat;
            Message = str;
        }
    }
}
