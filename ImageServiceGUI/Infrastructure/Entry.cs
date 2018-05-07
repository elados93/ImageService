using ImageService.Logging.Modal;
using System.Diagnostics;

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

        public static EventLogEntryType toEventLogEntryType(MessageTypeEnum type) 
        {
            switch ((int) type)
            {
                case 1: return EventLogEntryType.Error;
                case 2: return EventLogEntryType.Warning;
                case 4: return EventLogEntryType.Information;
                default: return EventLogEntryType.Information;
            }
        }

        public static MessageTypeEnum toMessageTypeEnum(EventLogEntryType type)
        {
            switch ((int) type)
            {
                case 1: return MessageTypeEnum.FAIL;
                case 2: return MessageTypeEnum.WARNING;
                case 4: return MessageTypeEnum.INFO;
                default: return MessageTypeEnum.INFO;
            }
        }
    }
}