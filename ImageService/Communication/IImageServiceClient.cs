using Communication;

namespace ImageService.Communication
{
    /// <summary>
    /// Interface for every service client.
    /// </summary>
    public interface IImageServiceClient
    {
        void recieveCommand();
        bool ClientConnected { get; set; }
    }
}
