using System;
using System.Configuration;
using System.Text;

namespace ImageService.AppConfig
{
    /// <summary>
    /// The class is used in order to parse the AppConfig file.
    /// </summary>
    public class AppConfigParser
    {
        public string handler;
        public string outputDir;
        public string sourceName;
        public string logName;
        public int thumbNailsSize;

        public AppConfigParser()
        {
            handler = ConfigurationManager.AppSettings["Handler"];
            outputDir = ConfigurationManager.AppSettings["OutputDir"];
            sourceName = ConfigurationManager.AppSettings["SourceName"];
            logName = ConfigurationManager.AppSettings["LogName"];
            thumbNailsSize = Int32.Parse(ConfigurationManager.AppSettings["ThumbnailSize"]);
        }
            
        /// <summary>
        /// gets the port number from the app Config.
        /// </summary>
        /// <param name="port"></param> is the number of the port that will be initialiesed during
        /// the function.
        /// <returns></returns>
        public static bool getPort(out int port)
        {
            return Int32.TryParse(ConfigurationManager.AppSettings["ServerPort"], out port);
        }

        /// <summary>
        /// this function gets a string of a handler that we have listened to, and now the client
        /// asked to stop listenning and remove it. then, this function updates the app config.
        /// after removing the specific handler it creates a "list" of the current
        /// handlers we still want to listen to
        /// </summary>
        /// <param name="path"></param> is the handler that we want to remove.
        /// <returns></returns>
        public static bool removeHandler(string path)
        {
            try
            {
                string handlersData = ConfigurationManager.AppSettings["Handler"];
                string[] arr = handlersData.Split(';');
                StringBuilder newString = new StringBuilder();
                foreach (string handler in arr)
                {
                    if (!handler.Equals(path))
                        newString.Append(handler + ';');
                }
                string update = newString.ToString().TrimEnd(';');

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("Handler");
                config.AppSettings.Settings.Add("Handler", update);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
