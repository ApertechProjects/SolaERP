using SolaERP.Application.Contracts.Services;
using SolaERP.Application.Dtos.User;
using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Persistence.Utils
{
    public class SendMailForRequestTemplates
    {
        public static void SendMail(List<UserList> users)
        {
            //var userREQP = users.Where(x => x.TemplateKey == EmailTemplateKey.REQP.ToString()).ToList();
            //if (userREQP.Count > 0)
            //{
            //    var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQP);
            //    await _mailService.SendMailForRequest(Response, templates, userREQP, EmailTemplateKey.REQP, model.Sequence, model.BusinessUnitName);
            //}
            //var userREQA = users.Where(x => x.TemplateKey == EmailTemplateKey.REQA.ToString()).ToList();
            //if (userREQA.Count > 0)
            //{
            //    var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQA);
            //    await _mailService.SendMailForRequest(Response, templates, userREQA, EmailTemplateKey.REQA, model.Sequence, model.BusinessUnitName);

            //}
            //var userREQR = users.Where(x => x.TemplateKey == EmailTemplateKey.REQR.ToString()).ToList();
            //if (userREQR.Count > 0)
            //{
            //    var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQR);
            //    await _mailService.SendMailForRequest(Response, templates, userREQR, EmailTemplateKey.REQR, model.Sequence, model.BusinessUnitName, model.RejectReason);
            //}
            //var userREQH = users.Where(x => x.TemplateKey == EmailTemplateKey.REQH.ToString()).ToList();
            //if (userREQH.Count > 0)
            //{
            //    var templates = await _emailNotificationService.GetEmailTemplateData(EmailTemplateKey.REQH);
            //    await _mailService.SendMailForRequest(Response, templates, userREQH, EmailTemplateKey.REQH, model.Sequence, model.BusinessUnitName);
            //}
        }
    }
}
