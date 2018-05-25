using Infrastructure;
using ImageServiceGUI.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    /// <summary>
    /// The log view model comunicate with the view and the model.
    /// </summary>
    class LogViewModel : INotifyPropertyChanged
    {
        private ILogModel logModel;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor of LogViewModel, creates the model and register the model 
        /// with propertyChanged.
        /// </summary>
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

        /// <summary>
        /// Notify that a property with the name "name" has changed.
        /// </summary>
        /// <param name="name">The name of the changed property</param>
        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

    }
}
