using Newtonsoft.Json.Linq;
using System.Linq;

namespace Communication
{
    public delegate void UpdateResponseArrived(MessageCommand responseObj);

    public class MessageCommand
    {
        public MessageCommand(int commandID, string[] args, string path)
        {
            CommandID = commandID;
            CommandArgs = args;
            RequestedDirPath = path;
        }

        public MessageCommand()
        {
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

            JObject cmdObj = JObject.Parse(str);
            int CommandID = (int)cmdObj["CommandID"];
            string RequestedDirPath = (string)cmdObj["RequestedDirPath"];
            JArray arr = (JArray)cmdObj["CommandArgs"];
            string[] array = arr.Select(c => (string)c).ToArray();
            MessageCommand msg = new MessageCommand(CommandID, array, RequestedDirPath);

            return msg;
        }
    }
}
