using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWebApplication.Models
{
    public class ConfigModel
    {
        public ConfigModel() {
            OutputDirectory = "elad aharons folder";
            SourceName = "shahar";
            LogName = "palmor";
            ThumbNailSize = 120;
        }

        public void copy(ConfigModel config)
        {
            OutputDirectory = config.OutputDirectory;
            SourceName = config.SourceName;
            LogName = config.LogName;
            ThumbNailSize = config.ThumbNailSize;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory")]
        public string OutputDirectory { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbNail Size")]
        public int ThumbNailSize { get; set; }

        // TODO: handlers??
    }
}