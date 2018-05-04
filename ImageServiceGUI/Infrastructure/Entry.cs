using ImageService.Logging.Modal;

namespace ImageServiceGUI.Model
{

    class Entry
    {

        public Entry(string message, MessageTypeEnum messageType)
        {
            this.Message = message;
            this.Type = messageType;
        }

        public MessageTypeEnum Type
        { get; set; }

        public string Message
        {
            get; set;
        }
    }
}