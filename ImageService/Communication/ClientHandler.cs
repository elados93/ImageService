using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal.Events;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class ClientHandler : IClientHandler
    {
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;

        private IImageController c_controller;
        private ILoggingService c_logging;

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
                        MessageCommand msg = MessageCommand.ParseJSON(requset);
                        if (msg != null)
                        {
                            CommandEnum command = (CommandEnum)msg.CommandID;
                            if (msg.RequestedDirPath != null) // If the command related to handler
                            {
                                string[] commandArgs = msg.CommandArgs;
                                string path = msg.RequestedDirPath;
                                CommandRecievedEventArgs c = new CommandRecievedEventArgs(command, commandArgs, path);

                                bool result;
                                string executionResult = c_controller.ExecuteCommand((CommandEnum)msg.CommandID, msg.CommandArgs, out result);
                                c_logging.Log("Command " + (CommandEnum)msg.CommandID + " was execute", MessageTypeEnum.INFO);
                                writer.Write(executionResult);

                                CommandRecieved?.Invoke(this, c);
                            }
                            else
                            {
                                bool result;
                                string executionResult = c_controller.ExecuteCommand((CommandEnum)msg.CommandID, msg.CommandArgs, out result);
                                c_logging.Log("Command " + (CommandEnum)msg.CommandID + " was execute", MessageTypeEnum.INFO);
                                writer.Write(executionResult);
                            }
                        }
                        else
                        {
                            writer.Write("Message validation failed");
                            c_logging.Log("Message validation failed", MessageTypeEnum.FAIL);
                        }

                    }
                    catch (Exception e)
                    {
                        c_logging.Log(e.Message, MessageTypeEnum.FAIL);
                    }
                    finally
                    {
                        client.Close();
                    }
                }
            }).Start();
        }
    }
}
