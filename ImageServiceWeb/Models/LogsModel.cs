﻿using Communication;
using ImageServiceGUI.Communication;
using Infrastracture.Enums;
using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;

namespace ImageServiceWeb.Models
{
    public delegate void VoidDelegate();
    public class LogsModel
    {
        public static IImageServiceClient imageServiceClient;
        private bool ifLogUpdate;


        /// <summary>
        /// constructor.
        /// </summary>
        public LogsModel()
        {
            imageServiceClient = ImageServiceClient.Instance;
            imageServiceClient.UpdateAllModels += updateLogs;

            Logs = new List<EntryLog>();
            ifLogUpdate = false;
            //if the service is connected send the command that gets the first logs.
            if (imageServiceClient.ClientConnected)
            {
                MessageCommand getFirstLosgs = new MessageCommand((int)CommandEnum.LogCommand, null, null);
                imageServiceClient.sendCommand(getFirstLosgs);
                //until the service didnt return an answer, and then update the view. 
                while (!ifLogUpdate)
                {
                    Thread.Sleep(100);
                }
            }
        }


        /// <summary>
        /// Updates the logs according to the answer that returned from the service.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void updateLogs(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            if (command == CommandEnum.UpdateNewLog) 
            {
                // Check if it's only one message log
                int result;
                if (!Int32.TryParse(msg.CommandArgs[0], out result))
                    Debug.WriteLine("Error parsing command type in parseLog");
                else
                {

                    EntryLog log = new EntryLog();
                    log.EntryType = ((MessageTypeEnum)result).ToString();
                    log.Message = msg.CommandArgs[1];
                    Logs.Insert(0, log);
                }
                //to stop the sleep
                ifLogUpdate = true;
            }
            //gets all the logs
            else
            {
                if (command == CommandEnum.LogCommand) 
                {
                    // Check if it's the first time and need to get every log entry.
                    if (msg.CommandArgs[0] != null)
                    { // Logs transfered success
                        List<Entry> recLog = JsonConvert.DeserializeObject<List<Entry>>(msg.CommandArgs[0]);
                        foreach (Entry entry in recLog)
                        {
                            EntryLog log = new EntryLog();
                            log.EntryType = entry.Type.ToString();
                            log.Message = entry.Message;
                            Logs.Insert(0, log);
                        }
                    }
                    else
                        Debug.WriteLine("Error get the first logs");
                    ifLogUpdate = true;
                }
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public List<EntryLog> Logs { get; set; }
    }
}