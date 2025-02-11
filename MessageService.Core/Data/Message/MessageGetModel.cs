namespace MessageService.Core.Data.Message
{
    public class MessageGetModel
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public int SeqNumber { get; set; }
        public MessageGetModel() { }
        public MessageGetModel(MessageEntity message)
        {
            Message = message.Message;
            Timestamp = message.Timestamp;
            SeqNumber = message.SeqNumber;
        }
    }
}
