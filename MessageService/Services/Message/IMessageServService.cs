using MessageService.Core.Data.Message;

namespace MessageService.Services.Message
{
    public interface IMessageServService
    {
        public Task<MessageEntity> AddMessageAsync (MessageEntity message);
        public Task<List<MessageGetModel>> GetMessagesAsync();
        public Task<List<MessageGetModel>> GetMessagesFromTime(DateTime timestampFrom, DateTime? timestampUpTo = null);
    }
}
