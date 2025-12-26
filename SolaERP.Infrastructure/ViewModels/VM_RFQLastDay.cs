using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels;

public class VM_RFQLastDay : VM_EmailTemplateBase
{
    private readonly string _vendorName;
    private readonly string _rfqNo;
    private readonly string _businessUnitName;
    private readonly DateTime _rfqDeadline;
    private readonly int _rfqId;
    private readonly IConfiguration _configuration;

    public VM_RFQLastDay(string lang, string vendorName, string rfqNo, DateTime rfqDeadline,
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
        get { return "RFQ Deadline Remind"; }
    }

    public string TemplateName()
    {
        return @"RFQLastDay.cshtml";
    }

    public string GetHeaderOfMailAz
    {
        get { return $"Xatırlatma: [{_rfqNo}] №-li sorğu üçün təqdimat müddəti yaxınlaşır"; }
    }

    public string GetHeaderOfMailEn
    {
        get { return $"Urgent: [{_rfqNo}] № Submission Deadline Approaching"; }
    }

    public HtmlString GetBodyOfMailAz()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Hörmətli {_vendorName},</p></td></tr>" +
            $"<tr><td><p>[{_rfqNo}] №-li RFQ üçün təqdimat müddətinin yaxınlaşdığına dair bir xatırlatmadır. Təklifinizi təqdim etmək üçün cəmi 24 saatınız qalıb. Xahiş edirik, sənədlərinizi tam şəkildə tamamlayıb son tarixdən əvvəl göndərməyinizi təmin edin.</p></td></tr>" +
            $"<tr><td><p>Son tarix: [{_rfqDeadline}].</p></td></tr>" +
            $"<tr><td><p>BusinessUnit - {_businessUnitName}</p></td></tr>" +
            "<tr><td><p>Hər hansı bir sualınız və ya köməyə ehtiyacınız olarsa, bizimlə əlaqə saxlamaqdan çəkinməyin.</p></td></tr>" +
            "<tr><td><p>Hörmətlə,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }

    public HtmlString GetBodyOfMailEn()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Dear {_vendorName},</p></td></tr>" +
            $"<tr><td><p>This is a friendly reminder that the submission deadline for the [{_rfqNo}] № RFQ is approaching. You have 24 hrs remaining to submit your proposal. Please ensure that your documents are completed and sent before the deadline.</p></td></tr>" +
            $"<tr><td><p>Deadline: [{_rfqDeadline}].</p></td></tr>" +
            $"<tr><td><p>BusinessUnit - {_businessUnitName}</p></td></tr>" +
            "<tr><td><p>Should you have any questions or require assistance, feel free to contact us.</p></td></tr>" +
            "<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }
}