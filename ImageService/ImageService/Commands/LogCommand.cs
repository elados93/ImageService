using ImageService.Commands;
using ImageService.Infrastructure.AppConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.ImageService.Commands
{
    class LogCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            AppConfigParser appConfig = new AppConfigParser();
            EventLog log = new EventLog(appConfig.logName, ".");
            EventLogEntryCollection entries = log.Entries;
            List<string> logEntries = new List<string>();
         
            foreach (EventLogEntry entry in entries)
            {
                logEntries.Add(entry.Message.ToString());
            }

            string convertEachString;
            if((convertEachString = JsonConvert.SerializeObject(logEntries)) == null)
            {
                result = false;
                return "convertion to string failed";
            }

            result = true;
            return convertEachString;
        }
    }
}
