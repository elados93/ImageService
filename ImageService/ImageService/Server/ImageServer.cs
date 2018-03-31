using System;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
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
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;            // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CloseService;               // The event that notifies the handlers about closing service.             
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
        }

        public void createHandler(string path)
        {
            IDirectoryHandler handler = new DirectoyHandler(path, m_controller, m_logging);
            CommandRecieved += handler.OnCommandRecieved;
            CloseService += handler.OnCloseService;
            handler.DirectoryClose += onCloseServer;
            handler.StartHandleDirectory(); // Start listening to events.
            m_logging.Log("Created Handler for path: " + path + " Yu-Pi-Do!", Logging.Modal.MessageTypeEnum.INFO);
        }

        // has no use currently
        public void sendCommand(CommandRecievedEventArgs args)
        {
            CommandRecieved?.Invoke(this, args);
        }

        public void onCloseServer(object sender, DirectoryCloseEventArgs args)
        {
            m_logging.Log(args.Message, Logging.Modal.MessageTypeEnum.INFO);
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            CommandRecieved -= handler.OnCommandRecieved;
        }

        public void onCloseService()
        {
            CloseService?.Invoke(this, null);
        }

    }
}
