using Communication;
using ImageServiceGUI.Model;
using Infrastracture.Enums;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
namespace ImageServiceGUI.ViewModel
{
    class SettingViewModel : INotifyPropertyChanged
    {
        private ISettingsModel settingModel;
        private string selectedItem;

        public event SendCommandToServer SendCommand;
        public event PropertyChangedEventHandler PropertyChanged;

        public SettingViewModel()
        {
            this.settingModel = new SettingsModel();
            settingModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };

            this.RemoveHandlerCommand = new DelegateCommand<object>(this.OnRemove, this.CanRemove);
            PropertyChanged += RemoveSelectedHandlerCommand;

            SendCommand += settingModel.SendByImageService;
        }

        private void RemoveSelectedHandlerCommand(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveHandlerCommand as DelegateCommand<object>;
            command?.RaiseCanExecuteChanged();
        }

        private void OnRemove(object obj)
        {
            string handlerToRemove = selectedItem;
            settingModel.Handlers.Remove(SelectedItem);
            SelectedItem = null;
            Debug.WriteLine("In On Remove" + vm_Handlers.ToString());
            string[] args = new string[1];
            args[0] = handlerToRemove;
            MessageCommand removeHandler = new MessageCommand((int)CommandEnum.CloseCommand, args, handlerToRemove);
            SendCommand?.Invoke(removeHandler);
        }

        private bool CanRemove(object obj)
        {
            return SelectedItem != null;
        }

        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<string> vm_Handlers { get { return settingModel.Handlers; } }

        public ICommand RemoveHandlerCommand { get; private set; }

        public string SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
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

        public int vm_ThumbnailsSize
        {
            get { return settingModel.ThumbnailsSize; }
        }

    }
}
