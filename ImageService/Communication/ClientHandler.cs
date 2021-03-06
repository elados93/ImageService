﻿using ImageService.Controller;
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
                                    closeGuiNotify(client, writer);
                                }
                                else
                                {
                                    if (msg.RequestedDirPath != null) // If the command related to handler
                                    {
                                        handelHandlersRequrest(msg);
                                    }
                                    else
                                    {
                                        // Not handler command
                                        bool result;
                                        string executionResult = c_controller.ExecuteCommand((CommandEnum)msg.CommandID, msg.CommandArgs, out result);

                                        Mutex.WaitOne();
                                        try
                                        {
                                            writer.Write(executionResult);
                                        } catch (Exception e)
                                        {
                                            // Can't send to client, remove the client from the list.
                                            ExcludeClient?.Invoke(client);
                                            Debug.WriteLine("Connection failded, client was excluded from list, error msg: " + e.Message);
                                        }
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

        private void closeGuiNotify(TcpClient client, BinaryWriter writer)
        {
            ExcludeClient?.Invoke(client);
            MessageCommand closeApproved = new MessageCommand((int)CommandEnum.ApprovedCloseGui, null, null);
            string closeApprovedString = JsonConvert.SerializeObject(closeApproved);
            Mutex.WaitOne();
            writer.Write(closeApprovedString);
            Mutex.ReleaseMutex();
        }

        private void handelHandlersRequrest(MessageCommand msg)
        {
            CommandEnum command = (CommandEnum)msg.CommandID;
            string[] commandArgs = msg.CommandArgs;
            string path = msg.RequestedDirPath;
            CommandRecievedEventArgs c = new CommandRecievedEventArgs(command, commandArgs, path);
            CommandRecieved?.Invoke(this, c); // Invoke ImageServer to deal with handler command
        }
    }
}
