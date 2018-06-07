using System;
using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Modal.Events;
using System.IO;
using ImageService.Communication;
using Infrastracture.Enums;
using Communication;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private IServer tcpServer;
        #endregion

        #region Events
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;            // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CloseService;               // The event that notifies the handlers about closing service.             
        public event UpdateResponseArrived UpdateResponse;                              // The event that notifies the Tcp Server about removing handler.

        #endregion

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            m_controller = controller;
            m_logging = logging;
            IClientHandler clientHandler = new ClientHandler(controller, logging);
            clientHandler.CommandRecieved += sendCommand;
            this.tcpServer = new TcpServer(clientHandler, logging);
            this.UpdateResponse += tcpServer.notifyAllClients;
            this.tcpServer.Start();
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
                m_logging.Log("Created Handler for path: " + path + " Yu-Pi-Do!", MessageTypeEnum.INFO);
            }
            else
            {
                m_logging.Log("The path:" + path + " isn't valid!", MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// this function gets the request and execute the relevent command. it checks if
        /// the message is relavent to all the handlers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param> is the massage to send with command request.
        public void sendCommand(object sender, CommandRecievedEventArgs args)
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
            MessageTypeEnum type = MessageTypeEnum.WARNING;
            IDirectoryHandler handler = (IDirectoryHandler)sender;
            try
            {
                CommandRecieved -= handler.OnCommandRecieved;
                CloseService -= handler.OnCloseService;
                message = "Handler for path: " + args.DirectoryPath + " was deleted from server";

                MessageCommand msg = new MessageCommand((int)CommandEnum.CloseCommand, null, args.DirectoryPath);
                UpdateResponse?.Invoke(msg);
            }
            catch
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
            tcpServer.Stop();
        }

        /// <summary>
        /// this function was registered to the update log message event in order to update tcp 
        /// server about a new log message that was recieved. the tcp sever will update all the clients.
        /// </summary>
        /// <param name="msg"></param>
        public void notifyTcpServer(MessageCommand msg)
        {
            tcpServer.notifyAllClients(msg);
        }
    }
}
