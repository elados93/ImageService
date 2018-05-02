using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    class LogModel : ILogModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private List<Tuple<string, string>> m_LogMessages;

        public List<Tuple<string, string>> LogMessages1
        {
            get { return m_LogMessages; } set
            {
                m_LogMessages = value;
                OnPropertyChanged("LogMessages1");
            }
        }
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
