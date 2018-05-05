using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class SingeltonServer : IServer
    {
        private static SingeltonServer instace;
        private int port;
        private TcpListener listener;
        private IClientHandler ch;

        private SingeltonServer(IClientHandler ch)
        {
            int tempPort;
            bool result = Infrastructure.AppConfig.AppConfigParser.getPort(out tempPort);
            if (result)
                this.port = tempPort;
            else
                throw new Exception("Can't parse port!");

            this.ch = ch;
        }

        public static SingeltonServer Instance(IClientHandler ch)
        {
            // TODO maybe get here...
            if (instace == null)
                instace = new SingeltonServer(ch);
            return instace;
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
                        Debug.WriteLine("Singelton server got new connection");
                        ch.HandleClient(client);
                    }
                    catch (SocketException e)
                    {
                        Debug.WriteLine("Singelton server was stopped Error: " + e.Message);
                        break;
                    }
                }
            });
            task.Start();
        }

        public void Stop()
        {
            listener.Stop();
            Debug.WriteLine("Singelton server was stopped");
        }

    }
}
