﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    interface IImageServiceClient
    {
        void sendCommand(MessageCommand commandRecievedEventArgs);
        void recieveCommand();
        void CloseClient();
    }
}
