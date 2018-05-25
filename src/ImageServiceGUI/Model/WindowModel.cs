using Communication;
using ImageServiceGUI.Communication;
using Infrastracture.Enums;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    /// <summary>
    /// Window model handels when the window will be closed.
    /// </summary>
    public class WindowModel : IWindowModel
    {
        private bool m_clientConnected; // Instance of a communication with the server.

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

        /// <summary>
        /// Constructor of Window model, also start the communication with the server.
        /// </summary>
        public WindowModel()
        {
            TcpClient = ImageServiceClient.Instance;
            ClientConnected = TcpClient.ClientConnected;
            TcpClient.UpdateAllModels += closeGui;
        }

        /// <summary>
        /// Funcion invoked by imageserviceclient, send command to the gui.
        /// </summary>
        /// <param name="msg">name of the message.</param>
        private void closeGui(MessageCommand msg)
        {
            if (msg.CommandID == (int)CommandEnum.ApprovedCloseGui)
                TcpClient.CloseClient();
        }

        /// <summary>
        /// Update "name" proprety was changed.
        /// </summary>
        /// <param name="name">Name of the property that changed.</param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
