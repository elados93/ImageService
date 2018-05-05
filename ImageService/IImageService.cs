using ImageService.Logging.Modal;

namespace ImageService
{
    public interface IImageService
    {
        void onMessage(object sender, MessageRecievedEventArgs args);
    }
}