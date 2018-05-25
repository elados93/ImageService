using ImageService.Modal;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }
        /// <summary>
        /// this function execute the relevent command by the right key of the controller dictionary.
        /// </summary>
        /// <param name="args"> is the path to the photo that we should deal with </param>
        /// <param name="result"> if the operation was successful or not </param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message
            return m_modal.AddFile(args[0], out result);
        }
    }
}
