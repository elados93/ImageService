using Communication;
using ImageService.Commands;
using ImageService.Communication;
using Infrastracture.Enums;
using System;
using System.Configuration;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        /// <summary>
        /// the excecution is to give the tcp client all the infromation that shows in the app config.
        /// it creats an array that is full with all the app config params. it convertes it to Json and 
        /// returns a string with all the info.
        /// </summary>
        /// <param name="args"></param> null
        /// <param name="result"></param> if the action was successful or not.
        /// <returns></returns> the string with all the info.
        public string Execute(string[] args, out bool result)
        {
            try
            {
                result = true;
                string[] arr = new string[5];
                arr[0] = ConfigurationManager.AppSettings.Get("Handler");
                arr[1] = ConfigurationManager.AppSettings.Get("OutputDir");
                arr[2] = ConfigurationManager.AppSettings.Get("SourceName");
                arr[3] = ConfigurationManager.AppSettings.Get("LogName");
                arr[4] = ConfigurationManager.AppSettings.Get("ThumbnailSize");

                // making a package with all the information about the app config.
                MessageCommand commandSendArgs = new MessageCommand((int)CommandEnum.GetConfigCommand, arr, null);
                return commandSendArgs.toJason();
            }
            catch (Exception ex)
            {
                result = false;
                return ex.ToString();
            }
        }
    }
}
