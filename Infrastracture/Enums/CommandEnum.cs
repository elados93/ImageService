namespace Infrastracture.Enums
{
    /// <summary>
    /// A simple enum for all of the commands.
    /// NewFileCommand: Create a new handler to monitor.
    /// GetConfigCommand: Get all of the AppConfig from the server as a jason.
    /// LogCommand: Send all of the logs entry as a jason.
    /// CloseCommand: Close a wanted handler from the service.
    /// UpdateNewLog: Send the new log message as a jason (one log message).
    /// ClosedGuiNotify: Notify the server that a GUI about to be closed.
    /// ApprovedCloseGui: Approved the close of the GUI by removing it as a client from the list.
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand,
        GetConfigCommand,
        LogCommand,
        CloseCommand,
        UpdateNewLog,
        ClosedGuiNotify,
        ApprovedCloseGui    }
}
