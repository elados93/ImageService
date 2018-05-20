using System;
using ImageService.Commands;
using ImageService.AppConfig;

namespace ImageService.Commands
{
    class DeleteHandlerFromAppConfigCommand : ICommand
    {
        public DeleteHandlerFromAppConfigCommand() { }
        
        /// <summary>
        /// gets the specific handler to remove and removes it from the app config.
        /// </summary>
        /// <param name="args"></param> is the param that hold the name of the handler.
        /// <param name="result"></param> flag that represents if the romoving was
        /// seccuessful or not.
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            result = AppConfigParser.removeHandler(args[0]); // Remove the wanted handler from AppConfig
            if (result)
                return args[0] + " was removed from AppConfig";
            else
                return args[0] + " was not removed from AppConfig";
        }
    }
}
