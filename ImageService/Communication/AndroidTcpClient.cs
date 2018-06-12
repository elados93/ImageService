using ImageService.AppConfig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    public class AndroidTcpClient : IImageServiceClient
    {
        private AndroidTcpClient() { Start(); }

        private void Start()
        {
            int port;
            AppConfigParser.getAndroidPort(out port);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("10.0.0.2"), port);
            TcpClient client = new TcpClient(ep);
            recieveCommand();
        }

        #region Members
        private bool m_clientConnected;
        private bool stopped;
        private TcpClient client;
        private static AndroidTcpClient instance;
        #endregion

        public event MessageTransfer handelPicture;

        public static AndroidTcpClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AndroidTcpClient();
                }
                return instance;
            }
        }

        public bool ClientConnected { get { return m_clientConnected; } set { m_clientConnected = value; } }

        private const int BUFFER_SIZE = 1024;

        public void recieveCommand()
        {
            new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);

                while (!stopped)
                {
                    try
                    {
                        //byte[] arrayOfBytes;
                        //byte b;

                        //String s = reader.ReadString();

                        byte[] buffer = new byte[BUFFER_SIZE];
                        int read = -1;
                        
                        while ((read = stream.Read(buffer, 0, BUFFER_SIZE)) > 0)
                        {
                                
                        }

                        //byte response = reader.ReadByte(); // Wait for response from server
                        
                        Debug.WriteLine($"Got message from Android: {buffer.ToString()} from Server");
                        handelPicture?.Invoke(buffer);

                        Thread.Sleep(100); // Update information every 0.1 seconds
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }).Start();
        }
    }
}
