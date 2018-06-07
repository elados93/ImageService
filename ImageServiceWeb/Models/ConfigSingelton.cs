using Communication;
using ImageServiceGUI.Communication;
using Infrastracture.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigSingelton
    {
        private static ConfigSingelton instance;

        #region private members
        private string m_outputdir;
        private string m_sourceName;
        private string m_logName;
        private int m_thumbnailsSize;
        private ObservableCollection<string> m_Handlers;
        private static bool gotUpdate;
        private static readonly object locker = new object(); // Notify when updates from server recived, instand of busy waiting
        private IImageServiceClient imageServiceClient;
        #endregion

        #region Properties
        public string OutputDirectory { get { return getWantedMember("outputDir"); } set { m_outputdir = value; } }

        public string SourceName { get { return getWantedMember("sourceName"); } set { m_sourceName = value; } }

        public string LogName { get { return getWantedMember("logName"); } set { m_logName = value; } }

        public int ThumbNailSize { get { return Convert.ToInt32(getWantedMember("thumbnailSize")); } set { m_thumbnailsSize = value; } }

        public ObservableCollection<string> Handlers
        {
            get
            {
                lock (locker)
                {
                    if (gotUpdate)
                        return m_Handlers;
                    getAppConfigFromServer();
                    return m_Handlers;
                }
            }
            set { m_Handlers = value; }
        }
        #endregion

        private ConfigSingelton()
        {
            imageServiceClient = ImageServiceClient.Instance;
            imageServiceClient.UpdateAllModels += updateConfig;
            gotUpdate = false;
            m_Handlers = new ObservableCollection<string>();
        }

        public static ConfigSingelton Instance
        {
            get
            {
                if (instance == null)
                    instance = new ConfigSingelton();
                return instance;
            }
        }

        private void getAppConfigFromServer()
        {
            MessageCommand requestAppConfig = new MessageCommand((int)CommandEnum.GetConfigCommand, null, null);
            imageServiceClient.sendCommand(requestAppConfig);

            while (!gotUpdate)
                Monitor.Wait(locker); // waits for the response from the service
        }

        private void updateConfig(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            if (command == CommandEnum.GetConfigCommand)
            {
                lock (locker)
                {
                    string[] args = msg.CommandArgs;
                    string handler = args[0]; // The args order is a convetion, as written in AppConfig.
                    m_outputdir = args[1];
                    m_sourceName = args[2];
                    m_logName = args[3];
                    int temp;
                    if (!Int32.TryParse(args[4], out temp))
                        Debug.WriteLine("Error parse thumbnail size in getAppConfig");
                    else
                        m_thumbnailsSize = temp;
                    insertHandlersToList(handler);
                    gotUpdate = true; // Update the output was arrived
                    Monitor.PulseAll(locker); // Exit wait status
                }
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
                m_Handlers.Add(handlerString);
        }

        public void RemoveHandler(string handler)
        {
            string[] args = new string[1];
            args[0] = handler;
            MessageCommand removeHandler = new MessageCommand((int)CommandEnum.CloseCommand, args, handler);
            imageServiceClient.sendCommand(removeHandler);
            m_Handlers.Remove(handler);
        }

        private string getWantedMember(string member)
        {
            lock (locker)
            {
                if (gotUpdate)
                {
                    return getMemberFromClass(member);
                }
                getAppConfigFromServer();
                return getMemberFromClass(member);
            }
        }

        private string getMemberFromClass(string member)
        {
            switch (member)
            {
                case "outputDir": return m_outputdir;
                case "logName": return m_logName;
                case "sourceName": return m_sourceName;
                case "thumbnailSize": return Convert.ToString(m_thumbnailsSize);
                default: return null;
            }
        }
    }
}