using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        private string m_handler;
        private string m_outputDir;
        private string m_sourceName;
        private string m_logName;
        private int m_thumbNailsSize;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        public int ThumbNailsSize { get => m_thumbNailsSize;
            set
            {
                m_thumbNailsSize = value;
                OnPropertyChanged("ThumbNailsSize");
            }
        }
        public string LogName { get => m_logName;
            set
            {
                m_logName = value;
                OnPropertyChanged("LogName");
            }
        }
        public string SourceName
        {
            get => m_sourceName; set
            {
                m_sourceName = value;
                OnPropertyChanged("SourceName");

            }
        }
        public string OutputDir
        {
            get => m_outputDir; set
            {
                m_outputDir = value;
                OnPropertyChanged("OutputDir");

            }
        }
        public string Handler
        {
            get => m_handler; set
            {
                m_handler = value;
                OnPropertyChanged("Handler");

            }
        }

      
    }
}
