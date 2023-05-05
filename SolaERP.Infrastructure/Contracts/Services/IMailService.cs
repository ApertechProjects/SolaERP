namespace SolaERP.Application.Contracts.Services
{
    public interface IMailService
    {
        Task<List<string>> SendSafeMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string code);
        Task<bool> SenEmailMessage<T>(string template, T viewModel, string to, string subject);
    }

}
