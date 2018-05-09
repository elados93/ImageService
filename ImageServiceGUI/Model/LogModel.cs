using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Communication;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Data;
using Infrastructure;
using Infrastracture.Enums;
using ImageServiceGUI.Communication;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Entry> m_LogMessages;

        private IImageServiceClient imageServiceClient;

        public LogModel()
        {
            m_LogMessages = new ObservableCollection<Entry>();
            Object locker = new Object();
            BindingOperations.EnableCollectionSynchronization(m_LogMessages, locker);

            imageServiceClient = ImageServiceClient.Instance; // ImageServiceClient is a singelton
            imageServiceClient.UpdateAllModels += parseToLog;
            if (imageServiceClient.ClientConnected)
                getFirstLogs();
        }


        private void getFirstLogs()
        {
            MessageCommand msg = new MessageCommand((int)CommandEnum.LogCommand, null, null);
            imageServiceClient.sendCommand(msg);
        }

        private void parseToLog(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            if (command == CommandEnum.UpdateNewLog) // Check if it's only one message log
            {
                int result;
                if (!Int32.TryParse(msg.CommandArgs[0], out result))
                    Debug.WriteLine("Error parsing command type in parseLog");
                else
                    m_LogMessages.Insert(0, new Entry(msg.CommandArgs[1], (MessageTypeEnum)result));
            }
            else
            {
                if (command == CommandEnum.LogCommand)
                {
                    if (msg.CommandArgs[0] != null)
                    { // Logs transfered success
                        List<Entry> recLog = JsonConvert.DeserializeObject<List<Entry>>(msg.CommandArgs[0]);
                        foreach (Entry entry in recLog)
                            m_LogMessages.Insert(0, entry); // Add the new entry first
                    }
                    else
                        Debug.WriteLine("Error get the first logs");
                }
            }
        }

        public ObservableCollection<Entry> LogMessages
        {
            get { return m_LogMessages; }
            set
            {
                m_LogMessages = value;
                OnPropertyChanged("LogMessages");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
