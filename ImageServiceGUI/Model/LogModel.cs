using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using ImageService.Logging.Modal;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Entry> m_LogMessages;

        public LogModel()
        {
            m_LogMessages = new ObservableCollection<Entry>();
            m_LogMessages.Add(new Entry("HeY!", MessageTypeEnum.INFO));
            m_LogMessages.Add(new Entry("ggggeY!", MessageTypeEnum.WARNING));
            m_LogMessages.Add(new Entry("HefdgdfgdfgdfY!", MessageTypeEnum.FAIL));
        }

        public ObservableCollection<Entry> LogMessages
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
