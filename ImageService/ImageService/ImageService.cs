﻿using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Runtime.InteropServices;
using ImageService.Server;
using ImageService.Controller;
using ImageService.Modal;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Infrastracture.Enums;
using Communication;
using ImageService.AppConfig;
using System.Threading;
using Infrastructure;

namespace ImageService
{

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };

    public partial class ImageService : ServiceBase
    {
        #region Members
        private int eventId = 1;
        private ILoggingService logger;
        private ImageServer m_imageServer;          // The Image Server
        private IImageServiceModal modal;
        private IImageController controller;
        #endregion

        #region Events
        public event UpdateResponseArrived UpdateLogMessage;
        #endregion

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public ImageService(string[] args)
        {
            try
            {
                InitializeComponent();
                AppConfigParser appConfigParser = new AppConfigParser();
                string logName = appConfigParser.logName;
                string eventSourceName = appConfigParser.sourceName;
                if (args.Count() > 0)
                {
                    eventSourceName = args[0];
                }
                if (args.Count() > 1)
                {
                    logName = args[1];
                }
                eventLog1 = new System.Diagnostics.EventLog();
                if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
                {
                    System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
                }
                eventLog1.Source = eventSourceName;
                eventLog1.Log = logName;

                logger = new LoggingService();
                logger.MessageRecieved += onMessage;

            } catch (Exception e)
            {
                eventLog1.WriteEntry(e.Message);
            }
        }

        /// <summary>
        /// Write the given message in event logger.
        /// </summary>
        /// <param name="sender">The object who called that function.</param>
        /// <param name="args">The message to be wrriten.</param>
        public void onMessage(object sender, MessageRecievedEventArgs args)
        {
            EventLogEntryType type = Entry.toEventLogEntryType(args.Status);
            eventLog1.WriteEntry(args.Message, type); // Write in the log of the service

            string []logArr = new string[2]; // Create array of strings represents the Log message
            logArr[0] = ((int)args.Status).ToString();
            logArr[1] = args.Message;
            MessageCommand msg = new MessageCommand((int)CommandEnum.UpdateNewLog, logArr, null);
            UpdateLogMessage?.Invoke(msg); // Notify the Tcp Server about the log command
        }

        /// <summary>
        /// The function parse AppConfig using AppConfigParser.
        /// </summary>
        /// <param name="appConfigParser">The AppConfigParser to work with, created before.</param>
        private void createObjects(AppConfigParser appConfigParser)
        {
            modal = new ImageServiceModal(appConfigParser.outputDir, appConfigParser.thumbNailsSize);
            controller = new ImageController(modal);
            m_imageServer = new ImageServer(controller, logger);
            UpdateLogMessage += m_imageServer.notifyTcpServer;
            string[] handlesPaths = appConfigParser.handler.Split(';');

            // Create all the handlers.
            foreach (string path in handlesPaths)
                m_imageServer.createHandler(path);

            AndroidModal androidModal;
            if (handlesPaths[0] != null)
                androidModal = new AndroidModal(handlesPaths[0], appConfigParser.outputDir);
        }

        protected override void OnStart(string[] args)
        {
            AppConfigParser appConfigParser = new AppConfigParser();

            // Create server, handlers, controller and modal.
            createObjects(appConfigParser);

            eventLog1.WriteEntry("In OnStart");

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {

            eventLog1.WriteEntry("In OnStop");

            // Update the service state to Stop Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Stopping.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Close all the handlers from server.
            m_imageServer.onCloseService();

            eventLog1.WriteEntry("Image Service stopped.");
            
            eventLog1.Clear();
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue");
        }

    }
}
