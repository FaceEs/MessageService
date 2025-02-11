namespace MessageService.Core.Data.Message
{
    public class MessageEntity : BaseEntity
    {
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public int SeqNumber { get; set; }
        public MessageEntity() {
            Timestamp = DateTime.UtcNow;
        }
        public MessageEntity(MessageAddModel model) {
            Id = new Guid();
            Message = model.Message;
            Timestamp = DateTime.UtcNow;
            SeqNumber = model.SeqNumber;
        }
    }
}
