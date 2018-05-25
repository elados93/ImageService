using Infrastracture.Enums;
using System.Diagnostics;

namespace Infrastructure
{
    /// <summary>
    /// this class represents each log massage. it contains the string of the massage and
    /// the type of it.
    /// </summary>
    public class Entry
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

        /// <summary>
        /// returning a massage type enum as a EventLogEntryType.
        /// </summary>
        /// <param name="type"></param> is the type according to the Message type enum.
        /// <returns></returns> the EventLogEntryType.
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

        /// <summary>
        /// convert the EventLogEntryType to MessageTypeEnum
        /// </summary>
        /// <param name="type"></param> is the type according to the EventLogEntryType.
        /// <returns></returns> MessageTypeEnum
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