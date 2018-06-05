
using Communication;
using ImageServiceGUI.Communication;
using Infrastracture.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading;
using System.Web.Mvc;

namespace ImageServiceWeb.Models
{
    public delegate ActionResult UpdateChange();
    public delegate void UpdatePhotos();

    public class ConfigModel
    {
 
        public IImageServiceClient imageServiceClient;
        private bool ifGetAppConfig;

        public ConfigModel()
        {
            imageServiceClient = ImageServiceClient.Instance;
            imageServiceClient.UpdateAllModels += updateConfig;
            imageServiceClient.UpdateAllModels += updateHandlerWasDeleted;

            OutputDirectory = "";
            SourceName = "";
            LogName = "";
            ThumbNailSize = 0;

            Handlers = new ObservableCollection<string>();
            ifGetAppConfig = false;

            if (imageServiceClient.ClientConnected)
            {
                MessageCommand requestAppConfig = new MessageCommand((int)CommandEnum.GetConfigCommand, null, null);
                imageServiceClient.sendCommand(requestAppConfig);
                while (!ifGetAppConfig)
                {
                    Thread.Sleep(100);
                }
            }
        }

        /// <summary>
        /// Gets the message with information about app config and saves it to the model.
        /// </summary>
        /// <param name="msg">The message with app config info.</param>
        private void updateConfig(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            if (command == CommandEnum.GetConfigCommand)
            {
                string[] args = msg.CommandArgs;
                string handler = args[0]; // The args order is a convetion, as written in AppConfig.
                OutputDirectory = args[1];
                SourceName = args[2];
                LogName = args[3];
                int temp;
                if (!Int32.TryParse(args[4], out temp))
                    Debug.WriteLine("Error parse thumbnail size in getAppConfig");
                else
                    ThumbNailSize = temp;
                insertHandlersToList(handler);
                ifGetAppConfig = true; // Update the output was arrived
            }
        }

        private void updateHandlerWasDeleted(MessageCommand deleteHandlerMessage)
        {
            // Do it only if the command is close command and a path was specified.
            if (deleteHandlerMessage.CommandID == (int)CommandEnum.CloseCommand &&
                deleteHandlerMessage.RequestedDirPath != null)
            {
                string pathToDelete = deleteHandlerMessage.RequestedDirPath;
                Handlers.Remove(pathToDelete);
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
                Handlers.Add(handlerString);
        }

        public void copy(ConfigModel config)
        {
            OutputDirectory = config.OutputDirectory;
            SourceName = config.SourceName;
            LogName = config.LogName;
            ThumbNailSize = config.ThumbNailSize;
            Handlers = config.Handlers;
        }

        public void RemoveHandler(string handler)
        {
            Handlers.Remove(handler);
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory: ")]
        public string OutputDirectory { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name: ")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name: ")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbNail Size: ")]
        public int ThumbNailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers List")]
        public ObservableCollection<string> Handlers { get; set; }
    }
}
