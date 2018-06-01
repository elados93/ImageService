
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        public ConfigModel()
        {
            OutputDirectory = "OutputDirectory!";
            SourceName = "SourceName!";
            LogName = "LogName!";
            ThumbNailSize = 120;
            Handlers = new ObservableCollection<string>();
            Handlers.Add("Chile");
            Handlers.Add("Brazil");
            Handlers.Add("Petach Tikva 2017");
        }

        public void copy(ConfigModel config)
        {
            OutputDirectory = config.OutputDirectory;
            SourceName = config.SourceName;
            LogName = config.LogName;
            ThumbNailSize = config.ThumbNailSize;
            Handlers = config.Handlers;
        }

        public void RemoveHandler(string handler)
        {
            Handlers.Remove(handler);
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Output Directory: ")]
        public string OutputDirectory { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Source Name: ")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Log Name: ")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbNail Size: ")]
        public int ThumbNailSize { get; set; }

        // TODO: handlers??
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Handlers { get; set; }

        
    }
}
