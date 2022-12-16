using SolaERP.Infrastructure.Dtos.Chat;
using SolaERP.Infrastructure.Entities.Auth;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IChatService
    {
        Task SaveChatHistoryAsync(ChatHistoryDto chatHistory);
        Task<List<ChatHistoryDto>> LoadChatHistoryAsync(int chatId);
        Task<User> GetSenderAsync(string finderToken);
    }
}
