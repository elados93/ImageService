using ImageServiceGUI.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    class SettingViewModel : INotifyPropertyChanged
    {
        private ISettingsModel settingModel;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingViewModel(ISettingsModel model)
        {
            this.settingModel = model;
            settingModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<string> vm_Handlers { get { return settingModel.Handlers; } }

        public string vm_OutputDir
        {
            get { return settingModel.OutputDir; }
        }

        public string vm_SourceName
        {
            get { return settingModel.SourceName; }
        }

        public string vm_LogName
        {
            get { return settingModel.LogName; }
        }

        public int vm_ThumbnailSize
        {
            get { return settingModel.ThumbNailsSize; }
        }

    }
}
