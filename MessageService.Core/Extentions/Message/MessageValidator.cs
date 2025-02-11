using MessageService.Core.Data.Message;

namespace MessageService.Core.Extentions.Message
{
    public static class MessageValidator
    {
        public static bool Valdiate(this MessageEntity message)
        {
            bool isValid = true;
            if(message.Message.Length > 128)
            {
                isValid = false;
            }
            return isValid;
        }
    }
}
