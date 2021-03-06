﻿using System;
using System.IO;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Events;
using ImageService.Server;
using Infrastracture.Enums;

namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        static readonly string[] extentions = { ".jpg", ".png", ".gif", ".bmp" };                // Will hold the extentions of all the files we will be monitoring.
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(string path, IImageController controller, ILoggingService logging)
        {
            m_logging = logging;
            m_controller = controller;
            m_path = path;
            m_dirWatcher = new FileSystemWatcher(path);
        }

        /// <summary>
        /// this function works on the existing files of the directory, and checks if
        /// there extentions are properly.
        /// </summary>
        public void StartHandleDirectory()
        {
            // sending a message to the event logger through the logging.
            m_logging.Log("Start to handle directory: " + m_path, MessageTypeEnum.INFO);
            // making sure the filesystem watcher litens to the specific directory if changes happens
            m_dirWatcher.Created += newFileCreation;
            m_dirWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        ///  When the watcher recognize a new image in the directory it sends the path to the handler
        ///  so it would be transported to the right output folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newFileCreation(object sender, FileSystemEventArgs e)
        {
            string[] args = { e.FullPath };
            if (checkFileExtention(e.FullPath))
                OnCommandRecieved(this, new CommandRecievedEventArgs(CommandEnum.NewFileCommand, args, m_path));
        }

        /// <summary>
        /// check if the extention is a proper extention to handle the specifc path.
        /// </summary>
        /// <param name="filePath"> is the path to check</param>
        /// <returns></returns>
        private bool checkFileExtention(string filePath)
        {
            string fileExtention = Path.GetExtension(filePath);
            bool isMatchExtention = false;
            foreach (string extention in extentions)
            {
                if (fileExtention.Equals(extention, StringComparison.CurrentCultureIgnoreCase))
                    isMatchExtention = true;
            }
            return isMatchExtention;
        }

        /// <summary>
        /// the event method that activates the controller by the right key
        /// and also sends aproper message to the logging if the treatment was successful or not.
        /// </summary>
        /// <param name="sender"> is the object that activates the events method.</param>
        /// <param name="e"> are the arguments needed to the action.</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.RequestDirPath.Equals(m_path))
            {
                if (e.CommandID == CommandEnum.CloseCommand)
                {
                    ImageServer server = (ImageServer)sender;
                    closeHandler(server);
                }
                bool result;
                string messageFromExecution = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);

                // Write the execution in the log.
                if (result)
                    m_logging.Log(messageFromExecution, MessageTypeEnum.INFO);
                else
                    m_logging.Log(messageFromExecution, MessageTypeEnum.FAIL);
            }
        }

        /// <summary>
        /// when the service is closed it sends a massage to all the handler that are signed and 
        /// tries to close each handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCloseService(object sender, CommandRecievedEventArgs e)
        {
            ImageServer imageServer = (ImageServer)sender;
            closeHandler(imageServer);
        }

        public void closeHandler(ImageServer imageServer)
        {
            try
            {
                // Making sure that when the service is closed the watcher will not listen anymore.
                m_dirWatcher.EnableRaisingEvents = false;
                m_dirWatcher.Dispose();
                m_logging.Log("Handler for path: " + m_path + " was closed", MessageTypeEnum.WARNING);
            }
            catch
            {
                m_logging.Log("Handler for path: " + m_path + " wasn't closed", MessageTypeEnum.FAIL);
            }
            finally
            {
                DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(m_path, "The handler"));
            }
        }

    }
}

