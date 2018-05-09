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
    public class WindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IWindowModel windowModel;

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

        private bool CanClose(object arg)
        {
            return true; // Can always close the window
        }

        private void OnClose(object obj)
        {
            windowModel.TcpClient.startClosingWindow();
        }

        public ICommand CloseWindowCommand { get; set; }

        public bool vm_clientConnected
        {
            get { return this.windowModel.ClientConnected; }

        }


        protected void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
