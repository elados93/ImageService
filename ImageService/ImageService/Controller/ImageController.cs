﻿using ImageService.Commands;
using ImageService.Modal;
using System.Collections.Generic;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        //members
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            //initializing the dictionary that state what kind of execution we can do.
            commands = new Dictionary<int, ICommand>()
            {
                { 1, new NewFileCommand(m_modal)}
            };
        }

        /// <summary>
        /// checks if the commandID is a command in the controller dictionary and than
        /// tries to executes them.
        /// </summary>
        /// <param name="commandID"> might be aproper key in the dictionary</param>
        /// <param name="args"> arguments </param>
        /// <param name="resultSuccesful"> is the result of the execution.</param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            if (!commands.ContainsKey(commandID))
            {
                resultSuccesful = false;
                return "Command not found!";
            }
            return commands[commandID].Execute(args, out resultSuccesful);
        }
    }
}
