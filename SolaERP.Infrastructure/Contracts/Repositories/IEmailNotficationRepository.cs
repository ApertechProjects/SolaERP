using SolaERP.Application.Entities.Email;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IEmailNotificationRepository
    {
        Task<List<EmailNotification>> GetAllEmailNotificationsAsync();
        Task<bool> CreateAsync(EmailNotification model);
        Task<bool> UpdateAsync(EmailNotification model);
        Task<bool> DeleteAsync(int id);

    }
}
