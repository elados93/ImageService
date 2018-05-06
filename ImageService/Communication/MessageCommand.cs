using Newtonsoft.Json.Linq;
using System.Linq;

namespace ImageService.Communication
{
    public class MessageCommand
    {
        public MessageCommand(int commandID, string[] args, string path)
        {
            CommandID = commandID;
            CommandArgs = args;
            RequestedDirPath = path;                
        }

        public int CommandID { get; set; }

        public string[] CommandArgs { get; set; }

        public string RequestedDirPath { get; set; }

        public string toJason()
        {
            JObject cmdObj = new JObject();
            cmdObj["CommandID"] = CommandID;
            cmdObj["RequestedDirPath"] = RequestedDirPath;
            JArray args = new JArray(CommandArgs);
            cmdObj["CommandArgs"] = args;
            return cmdObj.ToString(); 
        }
        
        public static MessageCommand ParseJSON(string str)
        {
            MessageCommand msg = new MessageCommand();
            JObject cmdObj = JObject.Parse(str);
            msg.CommandID = (int)cmdObj["CommandID"];
            msg.RequestedDirPath = (string)cmdObj["RequestedDirPath"];
            JArray arr = (JArray)cmdObj["CommandArgs"];
            msg.CommandArgs = arr.Select(c => (string)c).ToArray();

            return msg;
        }
    }
}
