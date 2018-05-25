using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using ImageService.Modal.Events;
using System.Threading;
using Newtonsoft.Json;
using ImageService.AppConfig;
using Communication;
using ImageService.Logging;
using Infrastracture.Enums;

namespace ImageService.Communication
{
    class TcpServer : IServer
    {
        private int port;
        private TcpListener listener;
        private ILoggingService m_logger;
        private IClientHandler ch;
        private List<TcpClient> clientsList;
        private static Mutex mutex = new Mutex();

        public TcpServer(IClientHandler clientHandler, ILoggingService logger)
        {
            int tempPort;
            // Getting the port from the app config.
            bool result = AppConfigParser.getPort(out tempPort);
            if (result)
                port = tempPort;
            else
                throw new Exception("Can't parse port!");
            m_logger = logger;
            this.ch = clientHandler;
            this.ch.Mutex = mutex;
            this.ch.ExcludeClient += excludeClientFromList;
            clientsList = new List<TcpClient>();
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Debug.WriteLine("Tcp Server is waiting for connections...");
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        // accepting clients
                        TcpClient client = listener.AcceptTcpClient();
                        Debug.WriteLine("Tcp server got new connection");
                        clientsList.Add(client);
                        ch.HandleClient(client);
                    }
                    catch (SocketException e)
                    {
                        Debug.WriteLine("Tcp server was stopped Error: " + e.Message);
                        break;
                    }
                }
            });
            task.Start();
        }

        public void Stop()
        {
            listener.Stop();
            foreach (TcpClient c in clientsList)
                c.Close();
            clientsList.Clear();
            Debug.WriteLine("Tcp server was stopped");
        }

        public void notifyAllClients(MessageCommand message)
        {
            
            new Task(() =>
            {
                foreach (TcpClient client in clientsList)
                {
                    NetworkStream stream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(stream);
                    {
                        string messageInString = JsonConvert.SerializeObject(message);
                        mutex.WaitOne();
                        writer.Write(messageInString);
                        mutex.ReleaseMutex();
                    }
                }

            }).Start();
            
        }

        /// <summary>
        ///  Invoked when someone is activating it, removes a specific
        ///  client from the list.
        /// </summary>
        /// <param name="client">The client that needs to be removed from the list.</param> 
        private void excludeClientFromList(TcpClient client)
        {
            m_logger.Log("Gui Client closed..", MessageTypeEnum.WARNING);
            clientsList.Remove(client);
        }
    }
}
