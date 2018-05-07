﻿using ImageService.Infrastructure.AppConfig;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using ImageService.Modal.Events;
using System.Threading;

namespace ImageService.Communication
{
    class TcpServer : IServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;
        private List<TcpClient> clientsList;
        private static Mutex mutex = new Mutex();

        public TcpServer(IClientHandler ch)
        {
            int tempPort;
            bool result = AppConfigParser.getPort(out tempPort);
            if (result)
                this.port = tempPort;
            else
                throw new Exception("Can't parse port!");

            this.ch = ch;
            this.ch.Mutex = mutex;
            clientsList = new List<TcpClient>();
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);
            listener.Start();
            Debug.WriteLine("Singelton Server is waiting for connections...");
            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
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
            clientsList.Clear();
            Debug.WriteLine("Tcp server was stopped");
        }

        public void notifyAllClients(MessageCommand message)
        {
            new Task(() =>
            {
                foreach (TcpClient client in clientsList)
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        mutex.WaitOne();
                        writer.Write(message);
                        mutex.ReleaseMutex();
                    }
                }

            }).Start();
        }
    }
}