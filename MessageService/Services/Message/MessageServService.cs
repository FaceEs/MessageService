using MessageService.Core.Data.Message;
using MessageService.Core.Extentions.Exceptions;
using Npgsql;

namespace MessageService.Services.Message
{
    public class MessageServService : IMessageServService
    {
        private readonly ILogger<MessageServService> _logger;
        private readonly IConfiguration _configuration;
        private string connectionString;
        private NpgsqlConnection connection;
        public MessageServService(ILogger<MessageServService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("Default");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new DBException("MessageServise: Не удалось подключиться к базе данных. Строка подключения пуста или отсутствует.");
            }
            connection = new NpgsqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                throw new DBException($"MessageServise: Не удалось подключиться к базе данных. {ex.Message}");
            }
            
        }
        public async Task<MessageEntity> AddMessageAsync(MessageEntity message)
        {
            try
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"INSERT INTO Messages (seqnumber, message, sendedTime) VALUES ({message.SeqNumber}, {message.Message}, {message.Timestamp})";
                    await command.ExecuteNonQueryAsync();
                }
                return message;
            }
            catch (Exception ex)
            {
                throw new DBException($"MessageServise: {ex.Message}");
            }
        }

        public async Task<List<MessageGetModel>> GetMessagesFromTime(DateTime timestampFrom, DateTime? timestampUpTo = null)
        {
            try
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    string timestampUpToString = timestampUpTo == null ? "LOCALTIMESTAMP" : timestampUpTo.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    command.CommandText = $"SELECT seqnumber,message,sendedTime FROM Messages * WHERE sendedTime BETWEEN timestamp '{timestampFrom.ToString("yyyy-MM-dd HH:mm:ss")} AND {timestampUpToString}')";
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    var result = new List<MessageGetModel>();
                    while (await reader.ReadAsync())
                    {
                        MessageGetModel message = new MessageGetModel();
                        message.SeqNumber = (int)reader["seqnumber"];
                        message.Message = (string)reader["message"];
                        message.Timestamp = (DateTime)reader["sendedTime"];
                        result.Add(message);
                    }
                    return result;
                }
                
            }
            catch (Exception ex)
            {
                throw new DBException($"MessageServise: {ex.Message}");
            }
        }

        public async Task<List<MessageGetModel>> GetMessagesAsync()
        {
            try
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"SELECT seqnumber,message,sendedTime FROM Messages)";
                    await command.ExecuteNonQueryAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    var result = new List<MessageGetModel>();
                    while (await reader.ReadAsync())
                    {
                        MessageGetModel message = new MessageGetModel();
                        message.SeqNumber = (int)reader["seqnumber"];
                        message.Message = (string)reader["message"];
                        message.Timestamp = (DateTime)reader["sendedTime"];
                        result.Add(message);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new DBException($"MessageServise: {ex.Message}");
            }
        }
    }
}
