using Infrastracture.Enums;
using System;

namespace ImageService.Modal.Events
{
    /// <summary>
    /// the arguments for the CommandRecievedEventArgs
    /// </summary>
    public class CommandRecievedEventArgs : EventArgs
    {
        public CommandEnum CommandID { get; set; }      // The Command ID
        public string[] Args { get; set; }
        public string RequestDirPath { get; set; }  // The Request Directory

        public CommandRecievedEventArgs(CommandEnum id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
