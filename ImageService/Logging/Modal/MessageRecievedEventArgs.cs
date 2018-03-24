using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    public class MessageRecievedEventArgs:EventArgs
    {
        public MessageRecievedEventArgs(MessageTypeEnum stat, String str)
        {
            Status = stat;
            Message = str;
        }

        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }
    }
}
