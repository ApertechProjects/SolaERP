using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels;

public class VM_RequestBuyer : VM_EmailTemplateBase
{
    private readonly string _buyerName;
    private readonly string _requestNo;
    private readonly string _businessUnitName;
    private readonly int _requestMainId;
    private readonly IConfiguration _configuration;

    public VM_RequestBuyer(string lang, string buyerName, string requestNo,
        int requestMainId, string businessUnitName)
    {
        string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
        IConfigurationBuilder builder =
            new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

        _configuration = builder.Build();
        _buyerName = buyerName;
        _requestNo = requestNo;
        _requestMainId = requestMainId;
        _businessUnitName = businessUnitName;
    }

    public string Subject
    {
        get { return "Request Buyer Assigned"; }
    }

    public string TemplateName()
    {
        return @"RequestBuyer.cshtml";
    }

    public string GetHeaderOfMailAz
    {
        get { return $" [{_requestNo}] №-li sorğu üçün alıcıya təyin olunmusunuz"; }
    }

    public string GetHeaderOfMailEn
    {
        get { return $" [{_requestNo}] № assigned buyer to you"; }
    }

    public HtmlString GetBodyOfMailAz()
    {
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Hörmətli {_buyerName},</p></td></tr>" +
            $"<tr><td><p>[{_requestNo}] №-li Sorğu üçün alıcıya təyin olunmusunuz.</p></td></tr>" +
            
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
            $"<tr><td><p>Dear {_buyerName},</p></td></tr>" +
            $"<tr><td><p> [{_requestNo}] № Request was assigned to you.</p></td></tr>" +
            
            $"<tr><td><p>BusinessUnit - {_businessUnitName}</p></td></tr>" +
            "<tr><td><p>Should you have any questions or require assistance, feel free to contact us.</p></td></tr>" +
            "<tr><td><p>Best regards,<br>GL Group</p></td></tr>" +
            "</table>"
        );
    }
}