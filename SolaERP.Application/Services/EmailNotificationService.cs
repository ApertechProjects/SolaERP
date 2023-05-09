using SolaERP.Application.Contracts.Repositories;
using SolaERP.Application.Contracts.Services;

namespace SolaERP.Persistence.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        IEmailNotificationRepository _emailNotificationRepository;
        public EmailNotificationService(IEmailNotificationRepository emailNotificationRepository)
        {
            _emailNotificationRepository = emailNotificationRepository;
        }
        public async Task<string> GetCompanyName(string email)
        {
            var res = await _emailNotificationRepository.GetCompanyName(email);
            return res;
        }
    }
}
