using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Communication;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Data;
using Infrastructure;
using Infrastracture.Enums;
using ImageServiceGUI.Communication;

namespace ImageServiceGUI.Model
{
    /// <summary>
    /// Log model is used to handel all the logs in the mvvm. Communicate with the server as well with ImageServiceClient.
    /// </summary>
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged; // The event that handels every change in the properties.
        private ObservableCollection<Entry> m_LogMessages; // The list of every entry since the service got started.

        private IImageServiceClient imageServiceClient; // Used to send and got messages from server.

        /// <summary>
        /// Constructor of Log Model, build the instance of log model and start communication with the server.
        /// </summary>
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

        /// <summary>
        /// Get the first logs from server, after that the server update each log separtley.
        /// </summary>
        private void getFirstLogs()
        {
            MessageCommand msg = new MessageCommand((int)CommandEnum.LogCommand, null, null);
            imageServiceClient.sendCommand(msg);
        }

        /// <summary>
        /// Gets the message that got from server and parse it to log entries.
        /// </summary>
        /// <param name="msg">The message got from server.</param>
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
                if (command == CommandEnum.LogCommand) // Check if it's the first time and need to get every log entry.
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
