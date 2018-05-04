using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    class SettingsModel : ISettingsModel
    {
        #region Members
        private string m_outputDir;
        private string m_sourceName;
        private string m_logName;
        private int m_thumbNailsSize;
        #endregion

        public ObservableCollection<string> Handlers { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsModel()
        {
            Handlers = new ObservableCollection<string>();
            Handlers.Add("dafsdf");
            Handlers.Add("HYHYHY");
            Handlers.Add("shafar");
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        public int ThumbNailsSize
        {
            get { return m_thumbNailsSize; }
            set
            {
                m_thumbNailsSize = value;
                OnPropertyChanged("ThumbNailsSize");
            }
        }
        public string LogName
        {
            get { return m_logName; }
            set
            {
                m_logName = value;
                OnPropertyChanged("LogName");
            }
        }
        public string SourceName
        {
            get { return m_sourceName; }
            set
            {
                m_sourceName = value;
                OnPropertyChanged("SourceName");

            }
        }

        public string OutputDir
        {
            get { return m_outputDir; }
            set
            {
                m_outputDir = value;
                OnPropertyChanged("OutputDir");

            }
        }

    }
}
