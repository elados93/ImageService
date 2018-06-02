using Communication;
using ImageServiceGUI.Communication;
using Infrastracture.Enums;
using Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ImageServiceWeb.Models
{
    public class LogsModel
    {

        public static IImageServiceClient imageServiceClient;

        public event UpdateChange RefreshAfterUpdates;

        public LogsModel()
        {
            imageServiceClient = ImageServiceClient.Instance;
            imageServiceClient.UpdateAllModels += updateLogs;

            Logs = new ObservableCollection<Entry>();
            Logs.Add(new Entry("the output folder was created", MessageTypeEnum.INFO));
            Logs.Add(new Entry("the output folder was fail", MessageTypeEnum.FAIL));
            Logs.Add(new Entry("the output folder was wearing pants", MessageTypeEnum.WARNING));

            MessageCommand getFirstLosgs = new MessageCommand((int)CommandEnum.LogCommand, null, null);
            imageServiceClient.sendCommand(getFirstLosgs);
        }

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
                    Logs.Insert(0, new Entry(msg.CommandArgs[1], (MessageTypeEnum)result));
                RefreshAfterUpdates?.Invoke();
            }
            else
            {
                if (command == CommandEnum.LogCommand) 
                {
                    // Check if it's the first time and need to get every log entry.
                    if (msg.CommandArgs[0] != null)
                    { // Logs transfered success
                        List<Entry> recLog = JsonConvert.DeserializeObject<List<Entry>>(msg.CommandArgs[0]);
                        foreach (Entry entry in recLog)
                            Logs.Insert(0, entry); // Add the new entry first
                    }
                    else
                        Debug.WriteLine("Error get the first logs");
                    RefreshAfterUpdates?.Invoke();
                }
            }
        }

        //public void copy(LogsModel log)
        //{
        //    Logs = log.Logs;
        //}

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public ObservableCollection<Entry> Logs { get; set; }
    }
}