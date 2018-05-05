using System;
using System.Configuration;
using System.Text;

namespace ImageService.Infrastructure.AppConfig
{
    /// <summary>
    /// The class is used in order to parse the AppConfig file.
    /// </summary>
    class AppConfigParser
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

        public static bool getPort(out int port)
        {
            return Int32.TryParse(ConfigurationManager.AppSettings["Port"], out port);
        }

        public static bool removeHandler(string path)
        {
            try
            {
                string handlersData = ConfigurationManager.AppSettings["Handler"];
                string[] arr = handlersData.Split(';');
                StringBuilder newString = new StringBuilder();
                foreach (string handler in arr)
                {
                    if (!handler.Equals("shaharpalmor!"))
                    {
                        newString.Append(handler + ';');
                    }
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
