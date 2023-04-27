using SolaERP.Application.Entities.Chat;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IChatRepository
    {
        Task<List<ChatHistory>> LoadChatHistoryAsync(int chatId);
        Task SaveChatHistoryAsync(ChatHistory chatHistory);
        Task SaveChatHistoriesAsync(List<ChatHistory> chatHistories);
    }
}
