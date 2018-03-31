using System.ServiceProcess;

namespace ImageService
{
    static class Program
    {
        /// <summary>
        /// The application start ImageService in order to backup wanted files from output
        /// folder as given in AppConfig.
        /// </summary>
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun = new ServiceBase[] { new ImageService(args) };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
