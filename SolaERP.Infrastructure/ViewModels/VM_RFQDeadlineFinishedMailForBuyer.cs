using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels
{
    public class VM_RFQDeadlineFinishedMailForBuyer : VM_EmailTemplateBase
    {
        private readonly string _buyerName;
        private readonly string _rfqNo;
        private readonly DateTime _rfqDeadline;
        private readonly int _rfqId;
        private readonly IConfiguration _configuration;

        public VM_RFQDeadlineFinishedMailForBuyer(string lang, string buyerName, string rfqNo, DateTime rfqDeadline,
            int rfqId)
        {
            _buyerName = buyerName;
            _rfqNo = rfqNo;
            _rfqDeadline = rfqDeadline;
            _rfqId = rfqId;
        }

        public string Subject
        {
            get { return "RFQ Deadline Finished"; }
        }

        public string TemplateName()
        {
            return @"RFQDeadlineFinishedMailForBuyer.cshtml";
        }

        public string GetHeaderOfMailAz
        {
            get { return "Qiymət Təklifi Sorğusunun son tarixi bitmişdir."; }
        }

        public string GetHeaderOfMailEn
        {
            get { return "RFQ Deadline Has Expired."; }
        }

        public HtmlString GetBodyOfMailAz()
        {
            return new HtmlString(
                "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
                $"<tr><td><p>Hörmətli {_buyerName},</p></td></tr>" +
                $"<tr><td><p>{_rfqNo} nömrəli Qiymət Təklifi Sorğusunun son müraciət tarixi {_rfqDeadline} tarixində bitmişdir.</p></td></tr>" +
                $"<tr><td><p>Sorğuya baxmaq üçün <b><a href={_configuration["Mail:ServerUrlUI"] + "/" + _rfqId} style='text-decoration: none;'> linkə </a></b> keçid edə bilərsiniz.</p></td></tr>" +
                "<tr><td><p>Hörmətlə,<br>GL Group</p></td></tr>" +
                "</table>"
            );
        }

        public HtmlString GetBodyOfMailEn()
        {
            return new HtmlString(
                "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
                $"<tr><td><p>Dear {_buyerName},</p></td></tr>" +
                $"<tr><td><p>The deadline for the submission of the {_rfqNo} number was {_rfqDeadline}.</p></td></tr>" +
                $"<tr><td><p>You can review the RFQ using the following <b><a href={_configuration["Mail:ServerUrlUI"] + "/" + _rfqId} style='text-decoration: none;'> link </a></b>.</p></td></tr>" +
                "<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
                "</table>"
            );
        }
    }
}