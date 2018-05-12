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
using System.Diagnostics;

namespace ImageService.Communication
{
    class ClientHandler : IClientHandler
    {
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        public event TcpClientDelegate ExcludeClient;

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
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                {

                    while (true)
                    {
                        try
                        {
                            string requset = reader.ReadString(); // Wait for command
                            MessageCommand msg = JsonConvert.DeserializeObject<MessageCommand>(requset);
                            if (msg != null)
                            {
                                CommandEnum command = (CommandEnum)msg.CommandID;

                                if (command == CommandEnum.ClosedGuiNotify)
                                {
                                    ExcludeClient?.Invoke(client);
                                    MessageCommand closeApproved = new MessageCommand((int)CommandEnum.ApprovedCloseGui, null, null);
                                    string closeApprovedString = JsonConvert.SerializeObject(closeApproved);
                                    Mutex.WaitOne();
                                    writer.Write(closeApprovedString);
                                    Mutex.ReleaseMutex();
                                }
                                else
                                {
                                    if (msg.RequestedDirPath != null) // If the command related to handler
                                    {
                                        string[] commandArgs = msg.CommandArgs;
                                        string path = msg.RequestedDirPath;
                                        CommandRecievedEventArgs c = new CommandRecievedEventArgs(command, commandArgs, path);
                                        CommandRecieved?.Invoke(this, c); // Invoke ImageServer to deal with handler command
                                    }
                                    else
                                    {
                                        // Not handler command
                                        bool result;
                                        string executionResult = c_controller.ExecuteCommand((CommandEnum)msg.CommandID, msg.CommandArgs, out result);
                                        Mutex.WaitOne();
                                        writer.Write(executionResult);
                                        Mutex.ReleaseMutex();
                                        
                                        if (result)
                                            c_logging.Log($"Command: {command}" + " success", MessageTypeEnum.INFO);
                                        else
                                            c_logging.Log($"Command: {command}" + " failed", MessageTypeEnum.FAIL);
                                    }

                                }
                            }
                            else // When the server got null message
                            { 
                                //Mutex.WaitOne();
                                // TODO create jason for error message
                                //writer.Write("Message validation failed"); 
                                //Mutex.ReleaseMutex();
                                Debug.WriteLine("Message validation failed");
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }
                    } // end while true
                }
            }).Start();
        }
    }
}
