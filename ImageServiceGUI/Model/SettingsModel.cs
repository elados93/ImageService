using ImageService.Communication;
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

        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            Object locker = new Object();
            BindingOperations.EnableCollectionSynchronization(Handlers, locker);
            imageServiceClient = ImageServiceClient.Instance; // Image service client is a singelton
            this.imageServiceClient.UpdateAllModels += getAppConfig;
            if (imageServiceClient.ClientConnected)
                getInitialAppConfig();
        }

        private void getInitialAppConfig()
        {
            MessageCommand msg = new MessageCommand((int)CommandEnum.GetConfigCommand, null, null);
            imageServiceClient.sendCommand(msg);
        }

        private void getAppConfig(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            if (command == CommandEnum.GetConfigCommand)
            {
                string[] args = msg.CommandArgs;
                string handler = args[0];
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

        private void insertHandlersToList(string handler)
        {
            string[] handlers = handler.Split(';');
            foreach (string handlerString in handlers)
                Handlers.Add(handlerString);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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
