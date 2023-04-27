using SolaERP.Application.Dtos.Chat;

namespace SolaERP.Application.Contracts.Services
{
    public interface IChatService
    {
        Task SaveChatHistoryAsync(ChatHistoryDto chatHistory);
        Task<List<ChatHistoryDto>> LoadChatHistoryAsync(int chatId);
    }
}
