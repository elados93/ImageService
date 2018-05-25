using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Diagnostics;
using Infrastracture.Enums;
using ImageServiceGUI.Communication;
using Communication;
using System.Windows.Data;

namespace ImageServiceGUI.Model
{
    /// <summary>
    /// Settings model is used to handel all the settings in the mvvm. Communicate with the server as well with ImageServiceClient.
    /// </summary>
    class SettingsModel : ISettingsModel
    {
        #region Members
        private string m_outputDir;
        private string m_sourceName;
        private string m_logName;
        private int m_thumbNailsSize;
        #endregion

        public ObservableCollection<string> Handlers { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private IImageServiceClient imageServiceClient;

        /// <summary>
        /// Constructor for settings model, start to communicate after creating the instance.
        /// </summary>
        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            Object locker = new Object();
            BindingOperations.EnableCollectionSynchronization(Handlers, locker);
            imageServiceClient = ImageServiceClient.Instance; // Image service client is a singelton
            this.imageServiceClient.UpdateAllModels += getAppConfig;
            this.imageServiceClient.UpdateAllModels += updateHandlerWasDeleted;
            if (imageServiceClient.ClientConnected)
                getInitialAppConfig();
        }

        private void updateHandlerWasDeleted(MessageCommand deleteHandlerMessage)
        {
            // Do it only if the command is close command and a path was specified.
            if (deleteHandlerMessage.CommandID == (int)CommandEnum.CloseCommand &&
                deleteHandlerMessage.RequestedDirPath != null)
            {
                string pathToDelete = deleteHandlerMessage.RequestedDirPath;
                Handlers.Remove(pathToDelete);
            }

        }

        /// <summary>
        /// Send the message that command the server to send the app config.
        /// </summary>
        private void getInitialAppConfig()
        {
            MessageCommand msg = new MessageCommand((int)CommandEnum.GetConfigCommand, null, null);
            imageServiceClient.sendCommand(msg);
        }

        /// <summary>
        /// Gets the message with information about app config and saves it to the model.
        /// </summary>
        /// <param name="msg">The message with app config info.</param>
        private void getAppConfig(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            if (command == CommandEnum.GetConfigCommand)
            {
                string[] args = msg.CommandArgs;
                string handler = args[0]; // The args order is a convetion, as written in AppConfig.
                OutputDir = args[1];
                SourceName = args[2];
                LogName = args[3];
                int temp;
                if (!Int32.TryParse(args[4], out temp))
                    Debug.WriteLine("Error parse thumbnail size in getAppConfig");
                else
                    ThumbnailsSize = temp;
                insertHandlersToList(handler);
            }
        }

        /// <summary>
        /// Insert the string "handler" to the data, split them by ;
        /// </summary>
        /// <param name="handler">The string of all handlers.</param>
        private void insertHandlersToList(string handler)
        {
            string[] handlers = handler.Split(';');
            foreach (string handlerString in handlers)
                Handlers.Add(handlerString);
        }

        /// <summary>
        /// Update the change of "name" was done.
        /// </summary>
        /// <param name="name">The property what changed.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Send the given message to server.
        /// </summary>
        /// <param name="msgToSend">The message to send.</param>
        public void SendByImageService(MessageCommand msgToSend)
        {
            imageServiceClient.sendCommand(msgToSend);
        }

        public int ThumbnailsSize
        {
            get { return m_thumbNailsSize; }
            set
            {
                m_thumbNailsSize = value;
                OnPropertyChanged("ThumbnailsSize");
            }
        }
        public string LogName
        {
            get { return m_logName; }
            set
            {
                m_logName = value;
                OnPropertyChanged("LogName");
            }
        }
        public string SourceName
        {
            get { return m_sourceName; }
            set
            {
                m_sourceName = value;
                OnPropertyChanged("SourceName");

            }
        }

        public string OutputDir
        {
            get { return m_outputDir; }
            set
            {
                m_outputDir = value;
                OnPropertyChanged("OutputDir");

            }
        }

    }
}
