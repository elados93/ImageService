using ImageServiceGUI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    class SettingViewModel : INotifyPropertyChanged
    {
        private SettingsModel settingModel;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public SettingViewModel()
        {
            this.settingModel = new SettingsModel();
            settingModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

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
