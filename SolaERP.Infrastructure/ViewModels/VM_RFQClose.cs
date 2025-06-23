using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels;

public class VM_RFQClose : VM_EmailTemplateBase
{
    private readonly string _vendorName;
    private readonly string _rfqNo;
    private readonly string _businessUnitName;
    private readonly DateTime _rfqDeadline;
    private readonly int _rfqId;
    private readonly IConfiguration _configuration;

    public VM_RFQClose(string lang, string vendorName, string rfqNo, DateTime rfqDeadline,
        int rfqId, string businessUnitName)
    {
        string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
        IConfigurationBuilder builder =
            new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

        _configuration = builder.Build();
        _vendorName = vendorName;
        _rfqNo = rfqNo;
        _rfqDeadline = rfqDeadline;
        _rfqId = rfqId;
        _businessUnitName = businessUnitName;
    }

    public string Subject
    {
        get { return "RFQ Close"; }
    }

    public string TemplateName()
    {
        return @"RFQClose.cshtml";
    }

    public string GetHeaderOfMailAz
    {
        get { return $"Təklifiniz növbəti mərhələyə keçmişdir – [{_rfqNo}]"; }
    }

    public string GetHeaderOfMailEn
    {
        get { return $"Your Proposal Has Been Forwarded – [{_rfqNo}]"; }
    }

    public HtmlString GetBodyOfMailAz()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Hörmətli {_vendorName},</p></td></tr>" +
            "<tr><td><p>Sizə bildiririk ki, RFQ üçün təqdimetmə müddəti rəsmi olaraq başa çatmışdır. Təklifiniz növbəti baxış mərhələsinə göndərilmişdir.</p></td></tr>" +
            $"<tr><td><p>RFQ Nomresi {_rfqNo} və RFQ son bitme tarixi {_rfqDeadline}.</p></td></tr>" +
            "<tr><td><p>Xahiş edirik təsdiq prosesinin nəticələrini gözləyin. Əlavə məlumat və ya aydınlaşdırma tələb olunarsa, sizinlə əlaqə saxlayacağıq.</p></td></tr>" +
            "<tr><td><p>İştirakınız və əməkdaşlığınız üçün təşəkkür edirik.</p></td></tr>" +
            "<tr><td><p>Hörmətlə,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }

    public HtmlString GetBodyOfMailEn()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Dear {_vendorName},</p></td></tr>" +
            "<tr><td><p>We would like to inform you that the submission period for the RFQ has officially closed. Your proposal has been successfully forwarded to the next stage of review.</p></td></tr>" +
            $"<tr><td><p>RFQ NO {_rfqNo} and RFQ Deadline {_rfqDeadline}.</p></td></tr>" +
            "<tr><td><p>Please await the results of this process. Should any additional information or clarification be required, we will reach out to you promptly.</p></td></tr>" +
            "<tr><td><p>Thank you for your participation and cooperation.</p></td></tr>" +
            "<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }
}