using System;
using ImageService.Commands;
using ImageService.AppConfig;

namespace ImageService.Commands
{
    class DeleteHandlerFromAppConfigCommand : ICommand
    {
        public DeleteHandlerFromAppConfigCommand() { }

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
