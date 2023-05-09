using SolaERP.Application.Entities.Email;
using SolaERP.Application.Enums;

namespace SolaERP.Application.Contracts.Repositories
{
    public interface IEmailNotificationRepository
    {
        Task<List<EmailNotification>> GetAllEmailNotificationsAsync();
        Task<bool> CreateAsync(EmailNotification model);
        Task<bool> UpdateAsync(EmailNotification model);
        Task<bool> DeleteAsync(int id);
        Task<string> GetCompanyName(string email);
        Task<EmailTemplateData> GetEmailTemplateDatas(Language language, EmailTemplateKey templateKey);

    }
}
