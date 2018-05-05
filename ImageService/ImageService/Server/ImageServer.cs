using System;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Modal.Events;
using System.IO;
using ImageService.Logging.Modal;
using ImageService.Communication;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private IServer singeltonServer;
        #endregion

        #region Events
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;            // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CloseService;               // The event that notifies the handlers about closing service.             
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
            singeltonServer = SingltonServer.Instance(new ClientHandler(controller, logging));
            singeltonServer.Start();
        }

        /// <summary>
        /// Create a wanted handler using the given path.
        /// </summary>
        /// <param name="path">The string to the directory to handle.</param>
        public void createHandler(string path)
        {
            if (Directory.Exists(path))
            {
                IDirectoryHandler handler = new DirectoyHandler(path, m_controller, m_logging);
                CommandRecieved += handler.OnCommandRecieved;
                CloseService += handler.OnCloseService;
                handler.DirectoryClose += deleteHandlerFromServer;

                // Start listening to events.
                handler.StartHandleDirectory();
                m_logging.Log("Created Handler for path: " + path + " Yu-Pi-Do!", Logging.Modal.MessageTypeEnum.INFO);
            }
            else
            {
                m_logging.Log("The path:" + path + " isn't valid!", MessageTypeEnum.FAIL);
            }
        }

        public void sendCommand(CommandRecievedEventArgs args)
        {
            CommandRecieved?.Invoke(this, args);
        }

        /// <summary>
        /// The function update the server that a handler is done handle the dir and
        /// server don't need to consider it anymore.
        /// </summary>
        /// <param name="sender">Some handler as object type.</param>
        /// <param name="args">Information about the closing dir.</param>
        public void deleteHandlerFromServer(object sender, DirectoryCloseEventArgs args)
        {
            String message = null;
            MessageTypeEnum type = MessageTypeEnum.INFO;
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            try
            {
                CommandRecieved -= handler.OnCommandRecieved;
                CloseService -= handler.OnCloseService;
                message = "Handler for path: " + args.DirectoryPath + " was deleted from server";
            } catch
            {
                type = MessageTypeEnum.FAIL;
                message = "Handler for path: " + args.DirectoryPath + " was NOT deleted from server";
            }
            finally
            {
                m_logging.Log(message, type);
            }

        }

        /// <summary>
        /// The function is called when the service is about to be closed so 
        /// the server close all his handlers using CloseService event.
        /// </summary>
        public void onCloseService()
        {
            CloseService?.Invoke(this, null);
            singeltonServer.Stop();
        }

    }
}
