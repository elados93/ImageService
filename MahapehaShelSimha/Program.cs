using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MahapehaShelSimha
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            if ((s = ConfigurationManager.AppSettings["OutputDir"]) != null) {
                string[] arr = s.Split(';');
                StringBuilder newString = new StringBuilder();
                foreach (string handler in arr)
                {
                    if (!handler.Equals("shaharpalmor!"))
                    {
                        newString.Append(handler + ';');
                    }
                }
                string update = newString.ToString();

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                // Add an Application Setting.
                config.AppSettings.Settings.Remove("Handler ");
                //config.AppSettings.Settings.Add("Handler", update);
                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Minimal);
                // Force a reload of a changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
        }
    }
}
