using Newtonsoft.Json.Linq;
using System.Linq;

namespace Communication
{
    // group of functions that updates when a response arrived.
    public delegate void UpdateResponseArrived(MessageCommand responseObj);

    /**
     * this class is the package that stores all the information that comes and goes to the 
     * tcp client, or to the client handler.
    */
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

        //properties
        public int CommandID { get; set; }

        public string[] CommandArgs { get; set; }

        public string RequestedDirPath { get; set; }

        /// <summary>
        /// This function creates a Json Object and takes each property of
        /// the class and turns it to a string so when it would be send it will be transfered
        /// as a string.
        /// </summary>
        /// <returns>The jason string.</returns>
        public string toJason()
        {
            JObject cmdObj = new JObject();
            cmdObj["CommandID"] = CommandID;
            cmdObj["RequestedDirPath"] = RequestedDirPath;
            JArray args = new JArray(CommandArgs);
            cmdObj["CommandArgs"] = args;
            return cmdObj.ToString();
        }
        
        /// <summary>
        /// this function het a string and turns it into a Message Command Object
        /// it transfer each part of the string using Json to the property of the package.
        /// </summary>
        /// <param name="str"></param> is the whole string
        /// <returns></returns>
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
