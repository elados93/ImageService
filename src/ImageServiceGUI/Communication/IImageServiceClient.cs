using Communication;

namespace ImageServiceGUI.Communication
{
    /// <summary>
    /// Interface for every service client.
    /// </summary>
    public interface IImageServiceClient
    {
        event UpdateResponseArrived UpdateAllModels;
        void sendCommand(MessageCommand commandRecievedEventArgs);
        void recieveCommand();
        void CloseClient();
        bool ClientConnected { get; set; }
        void startClosingWindow();
    }
}
