using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using ImageService.Modal.Events;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        
        #endregion

        #region Properties
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;          // The event that notifies about a new Command being recieved
        #endregion

        public void createHandler(string path)
        {
            IDirectoryHandler handler = new DirectoyHandler(path, m_controller);
            CommandRecieved += handler.OnCommandRecieved;
            handler.DirectoryClose += onCloseServer;
        }

        public void sendCommand(string message)
        {
            // invoke
        }

        public void onCloseServer(object sender, DirectoryCloseEventArgs args)
        {
            //-=
            m_logging.Log(args.Message, Logging.Modal.MessageTypeEnum.INFO);
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
        }

    }
}
