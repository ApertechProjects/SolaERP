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
        get { return $"{_requestNo} Request Assigned to You for Sourcing / Satınalma Tələbi Sizə Təhvil Verildi"; }
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
        string baseUrl = _configuration["Mail:ServerUrlUI"];
        string requestLink = $"{baseUrl}/requests/{_requestMainId}";
        
        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Hörmətli {_buyerName},</p></td></tr>" +
            "<tr><td><p>Siz satınalma prosesinə dair yeni Tələb üçün təyin olunmusunuz.</p></td></tr>" +
            $"<tr><td><p>Tələb Nömrəsi: {_requestNo}</p></td></tr>" +
            
            "<tr><td><p>Zəhmət olmasa satınalma fəaliyyətlərinə davam edin. Tələb haqqında bütün detallara aşağıdakı link vasitəsilə daxil ola bilərsiniz:</p></td></tr>" +
            $"<tr><td><p><a href='{requestLink}' style='color: #00008B; text-decoration: underline;'>{requestLink}</a></p></td></tr>" +
            "<tr><td><p>Hörmətlə,<br>SOLA ERP</p></td></tr>" +
            "</table>"
        );
    }

    public HtmlString GetBodyOfMailEn()
    {
        
        string baseUrl = _configuration["Mail:ServerUrlUI"];
        string requestLink = $"{baseUrl}/requests/{_requestMainId}";

        return new HtmlString(
            "<table width='100%' style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5; border-spacing: 0; padding: 0;'>" +
            $"<tr><td><p>Dear {_buyerName},</p></td></tr>" +
            "<tr><td><p> You have been assigned a new Request for the sourcing process.</p></td></tr>" +
            $"<tr><td><p> Request Number: {_requestNo}</p></td></tr>" +
            
            "<tr><td><p>Please proceed with the sourcing activities. You can access the full details of the Request via the following link:</p></td></tr>" +
            $"<tr><td><p><a href='{requestLink}' style='color: #00008B; text-decoration: underline;'>{requestLink}</a></p></td></tr>" +
            "<tr><td><p>Best regards,<br>SOLA ERP</p></td></tr>" +
            "</table>"
        );
    }
}