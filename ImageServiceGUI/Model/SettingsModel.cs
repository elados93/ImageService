using ImageService.Communication;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using ImageService.Logging.Modal;
using ImageService.Infrastructure.Enums;
using System.Diagnostics;

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
          //  imageServiceClient = ImageServiceClient.Instance; // Image service client is a singelton
           // this.imageServiceClient.UpdateAllClients += getAppConfig;
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
                    ThumbNailsSize = temp;
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


        public int ThumbNailsSize
        {
            get { return m_thumbNailsSize; }
            set
            {
                m_thumbNailsSize = value;
                OnPropertyChanged("ThumbNailsSize");
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
