using SolaERP.DataAccess.Extensions;
using SolaERP.Infrastructure.Contracts.Repositories;
using SolaERP.Infrastructure.Entities.Chat;
using SolaERP.Infrastructure.UnitOfWork;
using System.Data;
using System.Data.Common;

namespace SolaERP.DataAccess.DataAccess.SqlServer
{
    public class SqlChatRepository : IChatRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public SqlChatRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ChatHistory>> LoadChatHistoryAsync(int chatId)
        {
            List<ChatHistory> history = new List<ChatHistory>();
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_Load_Chat_History @ChatId";
                command.Parameters.AddWithValue(command, "@ChatId", chatId);

                using var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    history.Add(GetFromReader(reader));

                return history;
            };
        }


        public async Task SaveChatHistoriesAsync(List<ChatHistory> chatHistories)
        {
            foreach (var item in chatHistories)
            {
                await SaveChatHistoryAsync(item);
            }
        }

        public async Task SaveChatHistoryAsync(ChatHistory chatHistory)
        {
            using (var command = _unitOfWork.CreateCommand() as DbCommand)
            {
                command.CommandText = "SP_Insert_Chat_History";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue(command, "@SenderId", chatHistory.SenderId);
                command.Parameters.AddWithValue(command, "@ReceiverId", chatHistory.ReceiverId);
                command.Parameters.AddWithValue(command, "@Message", chatHistory.Message);
                command.Parameters.AddWithValue(command, "@MessageDate", chatHistory.MessageDate);

                await command.ExecuteNonQueryAsync();
            }
        }

        private ChatHistory GetFromReader(IDataReader reader)
        {
            return new ChatHistory
            {
                Id = reader.Get<int>("Id"),
                SenderId = reader.Get<int>("SenderId"),
                ReceiverId = reader.Get<int>("ReceiverId"),
                Message = reader.Get<string>("Message"),
                MessageDate = reader.Get<DateTime>("MessageDate"),
                Sender = new()
                {
                    Id = reader.Get<int>("Sender_Id"),
                    FullName = reader.Get<string>("SenderFullname"),
                    UserName = reader.Get<string>("SenderUsername"),
                    Photo = reader.Get<byte[]>("SenderPhoto"),
                    Email = reader.Get<string>("SenderEmail"),
                    UserToken = reader.Get<Guid>("SenderToken"),
                },
                Receiver = new()
                {
                    Id = reader.Get<int>("Receiver_Id"),
                    FullName = reader.Get<string>("ReceiverFullname"),
                    UserName = reader.Get<string>("ReceiverUsername"),
                    Photo = reader.Get<byte[]>("ReceiverPhoto"),
                    Email = reader.Get<string>("ReceiverEmail"),
                    UserToken = reader.Get<Guid>("ReceiverToken"),
                },

            };
        }

    }
}
