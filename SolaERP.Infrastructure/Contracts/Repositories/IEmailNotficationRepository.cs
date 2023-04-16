using SolaERP.Infrastructure.Entities.Email;

namespace SolaERP.Infrastructure.Contracts.Repositories
{
    public interface IEmailNotificationRepository
    {
        Task<List<EmailNotification>> GetAllEmailNotificationsAsync();
        Task<bool> CreateAsync(EmailNotification model);
        Task<bool> UpdateAsync(EmailNotification model);
        Task<bool> DeleteAsync(int id);

    }
}
