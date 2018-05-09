﻿using Communication;
using ImageService.AppConfig;
using ImageService.Communication;
using Infrastracture.Enums;
using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            try
            {
                AppConfigParser appConfig = new AppConfigParser();
                EventLog log = new EventLog(appConfig.logName, ".");
                EventLogEntryCollection entries = log.Entries;
                
                List<Entry> logEntries = new List<Entry>();
                foreach (EventLogEntry entry in entries)
                {
                    logEntries.Add(new Entry(entry.Message, Entry.toMessageTypeEnum(entry.EntryType)));
                }

                string convertEachString;
                if ((convertEachString = JsonConvert.SerializeObject(logEntries)) == null)
                {
                    result = false;
                    return null;
                }

                result = true;

                string[] arrForMsg = new string[1];
                arrForMsg[0] = convertEachString;
                MessageCommand msg = new MessageCommand((int)CommandEnum.LogCommand, arrForMsg, null);
                return JsonConvert.SerializeObject(msg);
            }
            catch (Exception e)
            {
                result = false;
                Debug.WriteLine(e.Message);
                return null;
            }

           
            
            
        }
    }
}
