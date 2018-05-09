using ImageService.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    public class WindowModel : IWindowModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool m_clientConnected;

        public bool ClientConnected
        {
            get { return this.m_clientConnected; }
            set
            {
                m_clientConnected = value;
                OnPropertyChanged("ClientConnected");
            }
        }

        public IImageServiceClient TcpClient { get; set; }

        public WindowModel()
        {
            //TODO
            //m_clientConnected = TcpClient.ClientConnected;
            TcpClient = ImageServiceClient.Instance;


        }


        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
