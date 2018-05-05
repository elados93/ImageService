using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class ClientHandler : IClientHandler
    {
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
                        string requset = reader.ReadString();
                        string[] requestArray = requset.Split(' ');
                        int commandID;
                        bool parseResult = int.TryParse(requestArray[0], out commandID);
                        if (parseResult)
                        {
                            bool result;
                            string executionResult = c_controller.ExecuteCommand((CommandEnum)commandID, requestArray, out result);
                            c_logging.Log("Command " + commandID + " was execute", MessageTypeEnum.INFO);
                            writer.Write(executionResult);
                        }
                        else
                        {
                            writer.Write("CommandID validation failed");
                            c_logging.Log("CommandID validation failed", MessageTypeEnum.FAIL);
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
