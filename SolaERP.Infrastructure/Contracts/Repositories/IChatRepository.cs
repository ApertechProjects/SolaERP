using SolaERP.Infrastructure.Entities.Chat;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IChatRepository
    {
        Task<List<ChatHistory>> LoadChatHistoryAsync(int chatId);
        Task SaveChatHistoryAsync(ChatHistory chatHistory);
        Task SaveChatHistoriesAsync(List<ChatHistory> chatHistories);
    }
}
