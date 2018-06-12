using Communication;

namespace ImageService.Communication
{
    public delegate void MessageTransfer(byte[] responseObj);

    /// <summary>
    /// Interface for every service client.
    /// </summary>
    public interface IImageServiceClient
    {
        event MessageTransfer handelPicture;
        void recieveCommand();
        bool ClientConnected { get; set; }
    }
}
