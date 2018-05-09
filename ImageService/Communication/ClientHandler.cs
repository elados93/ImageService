using ImageService.Controller;
using ImageService.Logging;
using ImageService.Modal.Events;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Infrastracture.Enums;
using Communication;

namespace ImageService.Communication
{
    class ClientHandler : IClientHandler
    {
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        private IImageController c_controller;
        private ILoggingService c_logging;
        public Mutex Mutex { get; set; }

        public ClientHandler(IImageController controller, ILoggingService logger)
        {
            c_controller = controller;
            c_logging = logger;
        }

        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    try
                    {
                        string requset = reader.ReadString(); // Wait for command
                        MessageCommand msg = JsonConvert.DeserializeObject<MessageCommand>(requset);
                        if (msg != null)
                        {
                            CommandEnum command = (CommandEnum)msg.CommandID;

                            if (msg.RequestedDirPath != null) // If the command related to handler
                            {
                                string[] commandArgs = msg.CommandArgs;
                                string path = msg.RequestedDirPath;
                                CommandRecievedEventArgs c = new CommandRecievedEventArgs(command, commandArgs, path);
                                CommandRecieved?.Invoke(this, c); // Invoke ImageServer to deal with handler command
                            }

                            // Not handler command
                            bool result;
                            string executionResult = c_controller.ExecuteCommand((CommandEnum)msg.CommandID, msg.CommandArgs, out result);

                            if (result)
                                c_logging.Log($"Command: {command}" + " success", MessageTypeEnum.INFO);
                            else
                                c_logging.Log($"Command: {command}" + " failed", MessageTypeEnum.FAIL);

                            Mutex.WaitOne();
                            writer.Write(executionResult);
                            Mutex.ReleaseMutex();
                        }
                        else
                        {
                            // When the server got null message
                            writer.Write("Message validation failed"); // TODO create jason for error message
                            c_logging.Log("Message validation failed", MessageTypeEnum.FAIL);
                        }
                    }
                    catch (Exception e)
                    {
                        c_logging.Log(e.Message, MessageTypeEnum.FAIL);
                    }
                    finally
                    {
                        //client.Close();
                    }
                }
            }).Start();
        }
    }
}
