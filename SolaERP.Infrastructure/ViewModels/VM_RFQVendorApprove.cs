using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using SolaERP.Infrastructure.ViewModels;

namespace SolaERP.Application.ViewModels;

public class VM_RFQVendorApprove : VM_EmailTemplateBase
{
    private readonly IConfiguration _configuration;
    private readonly string _lang;
    private readonly string _companyName;
    private readonly string _userEmail;
    
    public VM_RFQVendorApprove(string lang, string userEmail ,string companyName , int rfqId)
    {
        string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

        _configuration = builder.Build();
        _lang = lang;
        _userEmail = userEmail;
        _companyName = companyName;
    }
    
    public string Subject
    {
        get { return "A new RFQ request has been sent"; }
    }

    public string TemplateName()
    {
        return @"RFQVendorApprove.cshtml";
    }

    public string GetHeaderOfMailAz
    {
        get
        {
            return "Yeni RFQ sorğusu göndərildi";
        }
    }
    
    public string GetHeaderOfMailEn
    {
        get
        {
            return "A new RFQ request has been sent";
        }
    }
    
    public HtmlString GetBodyOfMailAz()
    {
        return new HtmlString(
            $"<div style='font-family: Arial, sans-serif; font-size: 14px; line-height: 1.5;'>"+
            $"<p>Hörmətli {_companyName},</p>" +
            "<p>Sizə yeni RFQ (Təklif Sorğusu) göndərilmişdir. Zəhmət olmasa, aşağıdakı linkə daxil olaraq sorğunu nəzərdən keçirin və müvafiq cavabınızı təqdim edin:</p>" +
            $"<p><b><a href='{_configuration["Mail:VendorServerUrlUI"]}/rfq-information' style='text-decoration: none;'>SolaERP</a></b></p>" +
            "<p>Əlavə suallarınız olarsa, bizimlə əlaqə saxlamaqdan çəkinməyin.</p>" +
            "<p>Hörmətlə,<br>GL Group</p>"
        );
    }
    
    public HtmlString GetBodyOfMailEn()
    {
        return new HtmlString(
            $"<p>Dear {_companyName},</p>" +
            "<p>A new RFQ (Request for Quotation) has been sent to you. Please click the link below to review the request and submit your response relatively:</p>" +
            $"<p><b><a href='{_configuration["Mail:VendorServerUrlUI"]}/rfq-information' style='text-decoration: none;'>SolaERP</a></b></p>" +
            "<p>If you have any further questions, please do not hesitate to contact us.</p>" +
            "<p>Best regards,<br>GL Group</p>" +
            "</div>"
        );
    }
    
}