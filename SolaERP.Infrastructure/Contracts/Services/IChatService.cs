using SolaERP.Infrastructure.Dtos.Chat;

namespace SolaERP.Infrastructure.Contracts.Services
{
    public interface IChatService
    {
        Task SaveChatHistoryAsync(ChatHistoryDto chatHistory);
        Task<List<ChatHistoryDto>> LoadChatHistoryAsync(int chatId);
    }
}
