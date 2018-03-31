using System;

namespace ImageService.Modal.Events
{
    /// <summary>
    /// the arguments for the DirectoryCloseEventArgs.
    /// </summary>
    public class DirectoryCloseEventArgs:EventArgs
    {
        public string DirectoryPath { get; set; }

        public string Message { get; set; }             // The Message That goes to the logger

        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }
    }
}
