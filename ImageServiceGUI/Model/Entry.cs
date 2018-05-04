


namespace ImageServiceGUI.Model
{

    public enum MessageTypeEnum
    {
        INFO = 4,
        WARNING = 2,
        FAIL = 1
    }

    class Entry
    {
        private MessageTypeEnum type;
        private string massage;

        public MessageTypeEnum Type { get => type; set => type = value; }
        public string Massage { get => massage; set => massage = value; }
    }
}
