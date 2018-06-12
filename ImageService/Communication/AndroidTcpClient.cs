using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class AndroidTcpClient : IImageServiceClient
    {
        private AndroidTcpClient() { }

        private static AndroidTcpClient instance;

        public AndroidTcpClient Instance
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

        public bool ClientConnected { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
                        string response = reader.ReadString(); // Wait for response from server
                        MessageCommand msg = JsonConvert.DeserializeObject<MessageCommand>(response);
                        Debug.WriteLine($"Got message: {msg.CommandID} from Server");
                        UpdateAllModels?.Invoke(msg);

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
