using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ImageServiceGUI.Communication
{
    public class ImageServiceClient : IImageServiceClient
    {
        private TcpClient client;
        private bool stopped;
        private static ImageServiceClient instance;

        public event UpdateResponseArrived UpdateAllClients;

        private static Mutex mutex = new Mutex();
        
    
        private ImageServiceClient() { Start(); }

        public static ImageServiceClient Instance
        {
            get
            {
                if (instance == null)
                    instance = new ImageServiceClient();
                return instance;
            }
        }

        public void Start()
        {
            try
            {
                int port;
                bool portResult = AppConfigParser.getPort(out port);
                if (!portResult)
                {
                    throw new Exception("Can't get port");
                }
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                client = new TcpClient();
                client.Connect(ep);
                Debug.WriteLine("Client connected");
                this.stopped = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public void sendCommand(MessageCommand msg)
        {
            new Task(() =>
            {
                string jsonCommand = JsonConvert.SerializeObject(msg);
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                mutex.WaitOne();
                writer.Write(jsonCommand);
                mutex.ReleaseMutex();
                Debug.WriteLine($"Send command: {msg.CommandID}" + $" args: {msg.CommandArgs.ToString()} to Server");
            }).Start();
        }

        public void recieveCommand()
        {
            new Task(() =>
           {
               NetworkStream stream = client.GetStream();
               BinaryReader reader = new BinaryReader(stream);
               string response = reader.ReadString(); // Wait for response from server
               MessageCommand msg = JsonConvert.DeserializeObject<MessageCommand>(response);
               Debug.WriteLine($"Got message: {msg.CommandID}" + $" args: {msg.CommandArgs.ToString()} to Server");
               UpdateAllClients?.Invoke(msg);
           }).Start();
        }

        public void CloseClient()
        {
            client.Close();
            this.stopped = true;
        }
    }
}
