using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IMailService
    {
        Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string code);
        Task<bool> SendEmailMessage<T>(string template, T viewModel, string to, string subject);
        Task<bool> SendUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName, List<string> tos);
        Task SendRequest(MailModel mailModel);
    }

}
