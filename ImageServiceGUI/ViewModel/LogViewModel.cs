using Infrastructure;
using ImageServiceGUI.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace ImageServiceGUI.ViewModel
{
    class LogViewModel : INotifyPropertyChanged
    {
        private ILogModel logModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public LogViewModel()
        {
            this.logModel = new LogModel();

            logModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };

        }

        public ObservableCollection<Entry> vm_LogMessages {
            get { return logModel.LogMessages; }
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
