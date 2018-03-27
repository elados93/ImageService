using System;
using System.IO;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Events;
using ImageService.Server;

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
        private DateTime lastRead;
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(string path, IImageController controller, ILoggingService logging)
        {
            m_logging = logging;
            m_controller = controller;
            m_path = path;
            m_dirWatcher = new FileSystemWatcher(path);
        }

        public void StartHandleDirectory()
        {
            // Scan the given path and handle each relavant file.
            string[] files = Directory.GetFiles(m_path);
            foreach (string filepath in files)
            {
                string[] args = { filepath };
                if (checkFileExtention(filepath))
                    OnCommandRecieved(this, new CommandRecievedEventArgs(1, args, filepath));
            }

            m_logging.Log("Start to handle directory: " + m_path, MessageTypeEnum.INFO);
            //m_dirWatcher.Created += new FileSystemEventHandler(newFileCreation);
            lastRead = DateTime.MinValue;
            m_dirWatcher.Changed += new FileSystemEventHandler(newFileCreation);
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
            DateTime lastWriteTime = File.GetLastWriteTime(e.FullPath);
            if (lastWriteTime != lastRead)
            {
                string[] args = { e.FullPath };
                if (checkFileExtention(e.FullPath))
                    OnCommandRecieved(this, new CommandRecievedEventArgs(1, args, m_path));
                lastRead = lastWriteTime;
            }
        }

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

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            bool result;
            string messageFromExecution = m_controller.ExecuteCommand(e.CommandID, e.Args, out result);

            // Write the execution in the log.
            if (result)
                m_logging.Log(messageFromExecution, MessageTypeEnum.INFO);
            else
                m_logging.Log(messageFromExecution, MessageTypeEnum.FAIL);
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
            try
            {
                m_dirWatcher.EnableRaisingEvents = false;
                m_logging.Log("Handler for path: " + m_path + " was closed", MessageTypeEnum.INFO);
            }
            catch
            {
                m_logging.Log("Handler for path: " + m_path + " wasn't closed", MessageTypeEnum.WARNING);
            }
            finally
            {
                imageServer.CommandRecieved -= this.OnCommandRecieved;
            }

        }
    }
}
