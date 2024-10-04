using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;

namespace SolaERP.Application.Contracts.Services
{
    public interface IMailService
    {
        Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendManualMailsAsync(string to);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string code);
        Task<bool> SendEmailMessage<T>(string template, T viewModel, string to, string subject);
        Task<bool> SendUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName, List<string> tos);
        Task SendRequestToMailService(MailModel mailModel);

        Task SendMailForRequest(HttpResponse response, List<EmailTemplateData> templates, List<UserList> users, EmailTemplateKey key, int? sequence, string businessUnitName, string rejectReason = "");
        Task SendRequestMailsForChangeStatus(HttpResponse response, List<UserList> users, int? sequence, string businessUnitName, string rejectReason);

        Task SendRegistrationPendingMail(int userId, EmailTemplateKey emailTemplateKey);

        Task SendMailToAdminstrationAboutRegistration(int userId, EmailTemplateKey emailTemplateKey);

        Task CheckLastApproveStageAndSendMailToVendor(int vendorId, int sequence, int approveStatus, HttpResponse response);
    }

}
