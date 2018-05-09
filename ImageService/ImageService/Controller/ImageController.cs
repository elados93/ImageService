using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Sockets;
using Infrastracture.Enums;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        #region Members
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<CommandEnum, ICommand> commands;
        #endregion

        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            //initializing the dictionary that state what kind of execution we can do.
            commands = new Dictionary<CommandEnum, ICommand>()
            {
                { CommandEnum.NewFileCommand, new NewFileCommand(m_modal)},
                { CommandEnum.CloseCommand, new DeleteHandlerFromAppConfigCommand()},
                { CommandEnum.LogCommand, new LogCommand() },
                {CommandEnum.GetConfigCommand, new GetConfigCommand() }
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
        public string ExecuteCommand(CommandEnum commandID, string[] args, out bool resultSuccesful)
        {
            if (!commands.ContainsKey(commandID))
            {
                resultSuccesful = false;
                return "Command not found!";
            }
            Task<Tuple<string, bool>> task = new Task<Tuple<string, bool>>(() =>
            {
                bool result;
                string msg = commands[commandID].Execute(args, out result);
                return Tuple.Create(msg, result);
            });
            task.Start();
            Tuple<string, bool> taskArgs = task.Result;
            resultSuccesful = taskArgs.Item2;
            return taskArgs.Item1;
        }
    }
}
