using SolaERP.Application.Contracts.Services;

namespace SolaERP.Infrastructure.Services
{
    public abstract class MailServiceBase : IMailService
    {



        public Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetMailAsync(string to, string code)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> SendSafeMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            throw new NotImplementedException();
        }

        public Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SenEmailMessage<T>(string template, T viewModel, string to, string subject)
        {
            throw new NotImplementedException();
        }



    }
}
