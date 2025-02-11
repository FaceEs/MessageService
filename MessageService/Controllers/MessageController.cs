using Microsoft.AspNetCore.Mvc;
using MessageService.Core.Data.Message;
using MessageService.Services.Message;
using MessageService.Core.Extentions;
namespace MessageService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MessageController : ControllerBase
    {

        private readonly ILogger<MessageController> _logger;
        private readonly IMessageServService _messageService;

        public MessageController(ILogger<MessageController> logger, IMessageServService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }
        /// <summary>
        /// Отправка сообщений
        /// </summary>
        /// <param name="message">Отправлямое сообщение</param>
        /// <returns>Отправленное сообщение</returns>
        [HttpPost(Name = "SendMessage")]
        public async Task<WebResponse<MessageGetModel>> SendMessage([FromBody] MessageAddModel message)
        {
            MessageEntity messageEntity = new MessageEntity(message);
            try
            {
                var response = await _messageService.AddMessageAsync(messageEntity);
                var getModel = new MessageGetModel(response);
                return new WebResponse<MessageGetModel>(getModel);
            }
            catch (Exception ex) {
                _logger.LogError($"Ошибка. {ex.Message}");
                WebResponse<MessageGetModel> errorResponse = new WebResponse<MessageGetModel>($"{ex.Message}",500);
                return errorResponse;
            }

        }
        /// <summary>
        /// Получение списка сообщений в заданном диапазоне дат
        /// </summary>
        /// <param name="dateFrom">Дата с которой начинать отсчет</param>
        /// <param name="dateUpTo">Дата до которой идёт отсчет. Оставить пустым для поиска до текущей даты</param>
        /// <param name="limit">Лимит выводимых сообщений на 1 странице. Для пагинации</param>
        /// <returns></returns>
        [HttpGet(Name = "GetMessagesByDate")]
        public async Task<WebResponse<List<MessageGetModel>>> GetMessagesByDate(DateTime dateFrom, DateTime? dateUpTo, int limit = 10)
        {
            try
            {
                var response = await _messageService.GetMessagesFromTime(dateFrom, dateUpTo);
                return new WebResponse<List<MessageGetModel>>(response, response.Count, limit);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка. {ex.Message}");
                WebResponse<List<MessageGetModel>> errorResponse = new WebResponse<List<MessageGetModel>>($"{ex.Message}", 500);
                return errorResponse;
            }
        }

    }
}
