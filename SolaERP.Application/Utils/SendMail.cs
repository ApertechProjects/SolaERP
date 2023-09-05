using SolaERP.Application.Entities.Email;

namespace SolaERP.Persistence.Utils
{
    public static class SendMail
    {
        public async static Task GetSendMailData(List<EmailTemplateData> emailTemplateDatas, List<string> tos)
        {
            //foreach (var lang in Enum.GetValues<Language>())
            //{
            //    if (tos.Count > 0)
            //    {
            //        var templateData = emailTemplateDatas.First(x => x.Language == lang.ToString());


            //        VM_RegistrationIsPendingAdminApprove adminApprove = GetVM(command, user, templateData);
            //        Task RegEmail = _mailService.SendUsingTemplate(templateData.Subject, adminApprove,
            //            adminApprove.TemplateName, adminApprove.ImageName, sendUserMails);
            //        emails.Add(RegEmail);
            //    }
            //}
        }
    }
}
