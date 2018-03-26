using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
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
        static readonly string[] extentions = { "jpg", "png", "gif", "bmp" };                // Will hold the extentions of all the files we will be monitoring.
        #endregion

        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        public DirectoyHandler(string path, IImageController controller, ILoggingService logging)
        {
            m_logging = logging;
            m_controller = controller;
            m_path = path;
            m_dirWatcher = new FileSystemWatcher(path);
        }

        // TODO implement that!
        public void StartHandleDirectory()
        {
            m_dirWatcher.EnableRaisingEvents = true;
            m_logging.Log("Start to handle directory: " + m_path, MessageTypeEnum.INFO);
            m_dirWatcher.Created += new FileSystemEventHandler(newFileCreation);
        }

        private void newFileCreation(object sender, FileSystemEventArgs e)
        {
            string fileExtention = Path.GetExtension(e.FullPath);
            bool isMatchExtention = false;
            foreach (string extention in extentions)
            {
                if (fileExtention.Equals(extention))
                    isMatchExtention = true;
            }
            if (isMatchExtention)
                OnCommandRecieved(this, new CommandRecievedEventArgs(1, null, m_path));

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

        public void OnCloseService(object sender, CommandRecievedEventArgs e)
        {
            ImageServer imageServer = (ImageServer)sender;
            try
            {
                m_dirWatcher.EnableRaisingEvents = false;
                m_logging.Log("Handler was closed", MessageTypeEnum.INFO);
            }
            catch
            {
                m_logging.Log("Handler wasn't closed", MessageTypeEnum.WARNING);
            }
            finally
            {
                imageServer.CommandRecieved -= this.OnCommandRecieved;
            }

        }
    }
}
