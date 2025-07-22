using Microsoft.AspNetCore.Http;
using SolaERP.Application.Dtos.Buyer;
using SolaERP.Application.Dtos.RFQ;
using SolaERP.Application.Entities.Email;
using SolaERP.Application.Entities.RFQ;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using UserList = SolaERP.Application.Dtos.User.UserList;

namespace SolaERP.Application.Contracts.Services
{
    public interface IMailService
    {
        Task SendSafeMailsAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendManualMailsAsync(string to);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string code);
        Task<bool> SendEmailMessage<T>(string template, T viewModel, string to, string subject);

        Task<bool> SendUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName,
            List<string> tos);

        Task SendRequestToMailService(MailModel mailModel);

        Task SendMailForRequest(HttpResponse response, List<EmailTemplateData> templates, List<UserList> users,
            EmailTemplateKey key, int? sequence, string businessUnitName, string rejectReason = "");

        Task SendRequestMailsForChangeStatus(HttpResponse response, List<UserList> users, int? sequence,
            string businessUnitName, string rejectReason);

        Task SendRegistrationPendingMail(int userId);


        Task CheckLastApproveStageAndSendMailToVendor(int vendorId, int sequence, int approveStatus,
            HttpResponse response);

        Task SendEmailVerification(HttpResponse response, int userId);
        Task SendMailToAdminstrationAboutRegistration(int userId);
        Task SendMailToAdminstrationForApproveRegistration(int userId, List<string> changedFields = null);
        Task SendMailToAdminstrationForApproveRegistrationForAutoApprove(int userId, List<string> changedFields = null);
        Task SendRejectMailToVendor(int vendorId, HttpResponse response);
        Task CheckLastApproveAndSendMailToUser(int userId, int sequence, int approveStatus, HttpResponse response);
        Task SendRejectMailToUser(int userId, HttpResponse response);
        Task SendMailToAdminstrationForApproveVendor(int vendorId);
        Task SendRFQDeadLineMail(List<RFQUserData> rfqUserData);
        Task SendRFQLastDayMail(List<RFQUserData> rfqUserData);
        Task SendSupportMail(int userId, string subject, string body, List<AttachmentSaveModel> attachments);
        
        Task SendQueueUsingTemplate<T>(string subject, T viewModel, string templateName, string imageName, List<string> tos);

        Task SendRFQVendorApproveMail(List<RfqVendorToSend> users);

        Task SendNewVendorApproveGroupEmail(List<string> emails, string vendorName);

        Task SendRFQDeadlineFinishedMailForBuyer(List<RFQDeadlineFinishedMailForBuyerDto> datas);
        Task BuyerPurchaseOrderApproveEmail(BuyerPurchaseOrderApproveEmailDto datas);
        Task RFQCloseSendVendorEmail(List<RFQVendorEmailDto> datas);
        Task RFQCloseSendVendorEmailForBCC(List<RFQVendorEmailDto> datas);

    }
}