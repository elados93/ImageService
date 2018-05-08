using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using ImageService.Logging.Modal;
using ImageService.Communication;
using System;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Entry> m_LogMessages;

        private IImageServiceClient imageServiceClient;
        private bool stopped;

        public LogModel()
        {
            m_LogMessages = new ObservableCollection<Entry>();
            //imageServiceClient = ImageServiceClient.Instance; // ImageServiceClient is a singelton
            //imageServiceClient.UpdateAllClients += parseToLog;
            //getFirstLogs();
            stopped = false;
            m_LogMessages.Add(new Entry("kjsbjks", MessageTypeEnum.FAIL));
            m_LogMessages.Add(new Entry("kjsbjks", MessageTypeEnum.WARNING));
            m_LogMessages.Add(new Entry("kjsbjks", MessageTypeEnum.INFO));
            //standByForNewLogs();
        }

        private void standByForNewLogs()
        {
            while (!stopped)
            {
                imageServiceClient.recieveCommand();
            }
        }

        private void getFirstLogs()
        {
            MessageCommand msg = new MessageCommand((int)CommandEnum.LogCommand, null, null);
            imageServiceClient.sendCommand(msg);
            MessageCommand recMessage = new MessageCommand();
            imageServiceClient.recieveCommand();
        }

        private void parseToLog(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            if (command == CommandEnum.UpdateNewLog)
            {
                int result;
                if (!Int32.TryParse(msg.CommandArgs[0], out result))
                    Debug.WriteLine("Error parsing command type in parseLog");
                else
                    m_LogMessages.Add(new Entry(msg.CommandArgs[1], (MessageTypeEnum)result));
            }
            else
            {
                if (command == CommandEnum.LogCommand)
                {
                    if (msg.CommandArgs[0] != null)
                    { // Logs transfered success
                        EventLogEntryCollection recLog = JsonConvert.DeserializeObject<EventLogEntryCollection>(msg.CommandArgs[0]);
                        foreach (EventLogEntry entry in recLog)
                            m_LogMessages.Add(new Entry(entry.Message, Entry.toMessageTypeEnum(entry.EntryType)));
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
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
