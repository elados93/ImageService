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
    /// <summary>
    /// The class SettingViewModel connects the view and the model, holding all the
    /// model's properties.
    /// </summary>
    class SettingViewModel : INotifyPropertyChanged
    {
        #region Members
        private ISettingsModel settingModel;
        private string selectedItem;
        #endregion

        #region Events
        public event SendCommandToServer SendCommand;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// Constructor of SettingViewModel, creates the setting's model with all his
        /// properties.
        /// </summary>
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

        /// <summary>
        /// Remove the wanted handler from the handler's list. Does this by invoking command.
        /// </summary>
        /// <param name="sender">The sender of the command.</param>
        /// <param name="e">The property that has changed.</param>
        private void RemoveSelectedHandlerCommand(object sender, PropertyChangedEventArgs e)
        {
            var command = this.RemoveHandlerCommand as DelegateCommand<object>;
            command?.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Remove the selected item from the handler's list.
        /// </summary>
        /// <param name="obj">Not used, only for delegation.</param>
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

        /// <summary>
        /// Check if the selected item can be removed.
        /// </summary>
        /// <param name="obj">Not used, only for delegation.</param>
        /// <returns>True or false if the selcted item is not null and can be deleted.</returns>
        private bool CanRemove(object obj)
        {
            return SelectedItem != null;
        }

        /// <summary>
        /// Notify the model that property has changed.
        /// </summary>
        /// <param name="name">The name of the property.</param>
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
