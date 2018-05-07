﻿using ImageService.Commands;
using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using System;
using System.Configuration;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
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