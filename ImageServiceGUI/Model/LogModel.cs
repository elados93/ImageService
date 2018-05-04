using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<EventLogEntry> m_LogMessages;

        public ObservableCollection<EventLogEntry> LogMessages
        {
            get { return m_LogMessages; }
            set
            {
                m_LogMessages = value;
                OnPropertyChanged("LogMessages");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
