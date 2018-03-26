﻿using ImageService.Modal;
using ImageService.Modal.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    public interface IDirectoryHandler
    {
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed
        void StartHandleDirectory();             // The Function Recieves the directory to Handle
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     // The Event that will be activated upon new Command
        void OnCloseService(object sender, CommandRecievedEventArgs e);     // The Event that will be activated when service close
    }
}
