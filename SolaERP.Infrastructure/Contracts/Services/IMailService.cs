namespace SolaERP.Application.Contracts.Services
{
    public interface IMailService
    {
        Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string code);
        Task<bool> SendEmailMessage(string template, string to, string subject);
    }

}
