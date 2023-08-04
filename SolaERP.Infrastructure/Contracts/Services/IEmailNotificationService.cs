using SolaERP.Application.Dtos.Email;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IEmailNotificationService
    {
        Task<string> GetCompanyName(string email);
        Task<EmailTemplateDataDto> GetEmailTemplateData(Language language, EmailTemplateKey templateKey);
        Task<List<EmailTemplateData>> GetEmailTemplateData(EmailTemplateKey templateKey);
        //Task<bool> SendVerificationEmail(UserRegisterModel dto);
    }
}
