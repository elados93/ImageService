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
        private static ImageServiceClient instance; // The instance of the singelton.

        public event UpdateResponseArrived UpdateAllModels; // This event update all the models that a message command recieved.

        private static Mutex mutex = new Mutex(); // Mutex to handle sync problems.

        /// <summary>
        /// The private construtor of ImageServiceClient, for the first and only instance start the service.
        /// </summary>
        private ImageServiceClient() { ClientConnected = Start(); }

        /// <summary>
        /// The public constructor of the service, The class is singelton.
        /// </summary>
        public static ImageServiceClient Instance
        {
            get
            {
                if (instance == null)
                    instance = new ImageServiceClient();
                return instance;
            }
        }

        /// <summary>
        /// Start the conecction with the server using the port from AppConfig. In the end
        /// start listing to calls.
        /// </summary>
        /// <returns>True or false if the start went well.</returns>
        public bool Start()
        {
            try
            {
                int port = 4444;
                //bool portResult = AppConfigParser.getPort(out port);
                //if (!portResult)
                //{
                //    throw new Exception("Can't get port");
                //}
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

        /// <summary>
        /// Send command via the service to the server, the communication is by Jason strings.
        /// </summary>
        /// <param name="msg">The message to send to the server.</param>
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

        /// <summary>
        /// Start recieving messages from server, this is a thread running as long as isStopped is false.
        /// </summary>
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
        
        /// <summary>
        /// Close the client propaly and set the right flags.
        /// </summary>
        public void CloseClient()
        {
            client.Close();
            this.stopped = true;
            ClientConnected = false;
        }

        /// <summary>
        /// The function that notify the server the window closed.
        /// </summary>
        public void startClosingWindow()
        {
            MessageCommand closeWindow = new MessageCommand((int)CommandEnum.ClosedGuiNotify, null, null);
            sendCommand(closeWindow);
        }

        public bool ClientConnected { get; set; }
    }
}
