using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace ImageServiceWeb.Models
{
    public class LogModel
    {
        public LogModel()
        {
            Logs = new ObservableCollection<string>();
            Logs.Add("the output folder was created");
            Logs.Add("On Start!");
            Logs.Add("On Close!");
        }

        public void copy(LogModel log)
        {
            Logs = log.Logs;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Logs")]
        public ObservableCollection<string> Logs { get; set; }
    }
}