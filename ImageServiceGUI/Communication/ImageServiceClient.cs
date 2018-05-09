using Communication;
using ImageService.AppConfig;
using Infrastracture.Enums;
using Infrastructure;
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

        public event UpdateResponseArrived UpdateAllModels;

        private static Mutex mutex = new Mutex();


        private ImageServiceClient() { ClientConnected = Start(); }

        public static ImageServiceClient Instance
        {
            get
            {
                if (instance == null)
                    instance = new ImageServiceClient();
                return instance;
            }
        }

        public bool Start()
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
                recieveCommand();   // Thread for recieving commands from server
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
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
                Debug.WriteLine($"Send command: {(CommandEnum)msg.CommandID} to Server");
            }).Start();
        }

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

                       Thread.Sleep(1000); // Update information every 1 seconds
                   }
                   catch (Exception e)
                   {
                       Debug.WriteLine(e.Message);
                   }
               }
           }).Start();
        }
        
        public void CloseClient()
        {
            client.Close();
            this.stopped = true;
            ClientConnected = false;
        }

        public void startClosingWindow()
        {
            MessageCommand closeWindow = new MessageCommand((int)CommandEnum.ClosedGuiNotify, null, null);
            sendCommand(closeWindow);
        }

        public bool ClientConnected { get; set; }
    }
}
