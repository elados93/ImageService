using Communication;

namespace ImageService.Communication
{
    public delegate void PictureHandel(string picName, byte[] byteArray);

    /// <summary>
    /// Interface for every service client.
    /// </summary>
    public interface IImageServiceClient
    {
        event PictureHandel handelPicture;
        void recieveCommand();
        bool ClientConnected { get; set; }
    }
}
