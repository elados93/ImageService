using Communication;
using ImageService.Communication;
using ImageServiceGUI.Communication;
using Infrastracture.Enums;
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
        private bool m_clientConnected;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ClientConnected
        {
            get { return TcpClient.ClientConnected; }
            set
            {
                m_clientConnected = value;
                OnPropertyChanged("ClientConnected");
            }
        }

        public IImageServiceClient TcpClient { get; set; }

        public WindowModel()
        {
            TcpClient = ImageServiceClient.Instance;
            ClientConnected = TcpClient.ClientConnected;
            TcpClient.UpdateAllModels += closeGui;
        }

        private void closeGui(MessageCommand msg)
        {
            if (msg.CommandID == (int)CommandEnum.ApprovedCloseGui)
                TcpClient.CloseClient();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
