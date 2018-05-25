using ImageServiceGUI.Model;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ImageServiceGUI.ViewModel
{
    /// <summary>
    /// The WindowViewModel is used to connect the view and the model of the window.
    /// </summary>
    public class WindowViewModel : INotifyPropertyChanged
    {
        private IWindowModel windowModel;

        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Constructor of the WindowViewModel, register the PropertyChanged event.
        /// </summary>
        public WindowViewModel()
        {
            windowModel = new WindowModel();
            windowModel.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
            };

            CloseWindowCommand = new DelegateCommand<object>(OnClose, CanClose);
        }

        /// <summary>
        /// For now the window always can be closed.
        /// </summary>
        /// <param name="arg">Not used.</param>
        /// <returns>True always, for now.</returns>
        private bool CanClose(object arg)
        {
            return true; // Can always close the window
        }

        /// <summary>
        /// Close the window of the GUI and report the server.
        /// </summary>
        /// <param name="obj">Not used.</param>
        private void OnClose(object obj)
        {
            windowModel.TcpClient.startClosingWindow();
        }

        public ICommand CloseWindowCommand { get; set; }

        public bool vm_clientConnected
        {
            get { return this.windowModel.ClientConnected; }

        }

        /// <summary>
        /// Notify a property has changed.
        /// </summary>
        /// <param name="name"></param>
        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
