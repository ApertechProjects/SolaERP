using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels;

public class VM_RFQCloseForBCC : VM_EmailTemplateBase
{
    private readonly string _vendorName;
    private readonly string _rfqNo;
    private readonly string _bidNo;
    private readonly string _businessUnitName;
    private readonly DateTime _rfqDeadline;
    private readonly int _rfqId;
    private readonly IConfiguration _configuration;

    public VM_RFQCloseForBCC(string lang, string vendorName, string rfqNo, DateTime rfqDeadline,
        int rfqId, string businessUnitName, string bidNo)
    {
        string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
        IConfigurationBuilder builder =
            new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

        _configuration = builder.Build();
        _vendorName = vendorName;
        _rfqNo = rfqNo;
        _bidNo = bidNo;
        _rfqDeadline = rfqDeadline;
        _rfqId = rfqId;
        _businessUnitName = businessUnitName;
    }

    public string Subject
    {
        get { return "Supplier notification"; }
    }

    public string TemplateName()
    {
        return @"RFQCloseForBCC.cshtml";
    }

    public string GetHeaderOfMailAz
    {
        get { return $"Təklif vermə müddətinin başa çatması barədə bildiriş – {_rfqNo}"; }
    }

    public string GetHeaderOfMailEn
    {
        get { return $"Notification of Bid Expiry – Reference No. {_rfqNo}"; }
    }

    public HtmlString GetBodyOfMailAz()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Hörmətli {_vendorName},</p></td></tr>" +
            $"<tr><td><p>Təklif - {_rfqNo} üzrə keçirilmiş son təklif vermə prosesində iştirak etdiyiniz üçün sizə təşəkkür edirik.</p></td></tr>" +
            "<tr><td><p>Bildiririk ki, təklif vermə müddəti artıq başa çatmışdır və bu mərhələdə yeni təkliflər qəbul edilmir.</p></td></tr>" +
            "<tr><td><p>Sizin təqdim etdiyiniz təklif şirkətimiz tərəfindən qəbul olunmuşdur və hal-hazırda satınalma və biznes komandaları tərəfindən nəzərdən keçirilir. Təfərrüatların işlənməsi üçün zəhmət olmasa 5–7 iş günü gözləyin.</p></td></tr>" +
            "<tr><td><p>Əgər təklifiniz uğurlu olarsa, bu barədə sizə ayrıca olaraq Satınalma Sifarişi (PO) göndəriləcək.</p></td></tr>" +
            "<tr><td><p>Əgər növbəti iki (2) həftə ərzində sizə hər hansı yenilənmiş məlumat təqdim edilməzsə, bu zaman təklifinizin bu mərhələdə seçilmədiyini nəzərə almağınızı xahiş edirik.</p></td></tr>" +
            "<tr><td><p>İştirakınıza görə bir daha təşəkkür edirik və gələcək tenderlərdə də fəal iştirakınızı görməkdən məmnun olarıq.</p></td></tr>" +
            "<tr><td><p>Hörmətlə,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }

    public HtmlString GetBodyOfMailEn()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Dear {_vendorName},</p></td></tr>" +
            $"<tr><td><p>Thank you for your participation in the recent bidding exercise under Bid Reference No. {_rfqNo}.</p></td></tr>" +
            "<tr><td><p>Please be informed that this bid has now expired and is no longer accepting proposals.</p></td></tr>" +
            "<tr><td><p>We confirm that your submitted proposal has been received by the company and under review with procurement and business teams. Please allow 5–7 working days for us to process the submitted details.</p></td></tr>" +
            "<tr><td><p>In case your bid is successful, you will receive a Purchase Order (PO) as a separate communication.</p></td></tr>" +
            "<tr><td><p>In case you do not receive any updates within the next two (2) weeks, kindly consider that your proposal was not selected on this occasion.</p></td></tr>" +
            "<tr><td><p>We sincerely thank you for your participation and encourage you to continue engaging in our future bidding exercises.</p></td></tr>" +
            "<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }
}