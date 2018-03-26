using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ImageService.Infrastructure.AppConfig
{
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

    }
}
