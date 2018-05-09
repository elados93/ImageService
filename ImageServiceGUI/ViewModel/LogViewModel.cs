using ImageService.Infrastructure;
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

        public LogViewModel(ILogModel model)
        {
            logModel = model;

            logModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };

        }

        public ObservableCollection<Entry> vm_LogMessages
        {
            get { return logModel.LogMessages; }
        }

        protected void NotifyPropertyChanged(string name)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
